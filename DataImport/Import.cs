using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Jupiter.Utility;
using System.Web;
using System.Net.Http;

namespace DataImport
{
    public class Import
    {
        private SqlDataAccess DA = new SqlDataAccess("connection");

        private const int FILE_SUFFIX_LENGTH = 5;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void Start(string fileName = null)
        {
            var allSettingRecords = DA.Query<Jupiter.DataModel.ServerImportSetting>(
                "SELECT [Source],[DataLoadTableName],[TargetTableName],[KeyColumns],[Columns],[Directory],[ArchiveFolder],[FilePattern] FROM [ImportSetting]");
            IEnumerable<Jupiter.DataModel.ServerImportSetting> settings;

            if (fileName != null)
            {
                //Find settings of which the file prefix matches
                settings = allSettingRecords.Where(p =>
                {
                    var pattern = p.FilePattern;
                    var filePrefix = pattern.Substring(0, pattern.Length - FILE_SUFFIX_LENGTH);
                    Log.Debug("fileName:[{0}],prefix:[{1}], [{2}]",
                        fileName,
                        filePrefix,
                        fileName.StartsWith(filePrefix));
                    return fileName.StartsWith(filePrefix);
                });
            }
            else
            {
                settings = allSettingRecords;
            }

            foreach (var setting in settings.ToList())
            {
                var files = Directory.GetFiles(setting.Directory, setting.FilePattern);

                if (files.Length == 0)
                {
                    continue;
                }

                var fileList = files.ToList();
                fileList.Sort();

                Log.Debug("All files sorted:" + string.Join("\r\n", fileList));

                foreach (var file in fileList)
                {
                    var fName = Path.GetFileName(file);
                    if (fileName != null && fName != fileName)
                    {
                        continue;
                    }

                    try
                    {
                        bulkCopy(file, setting.DataLoadTableName);

                        //1. Process data
                        DA.Execute("[SP_Import]",
                            new
                            {
                                dataloadTableName = setting.DataLoadTableName,
                                targetTableName = setting.TargetTableName,
                                keyColumns = setting.KeyColumns,
                                columns = setting.Columns,
                                source = setting.Source,
                                fileName = fName
                            });

                        //2. Archive file
                        var targetPath = string.Format("{0}{1}", setting.ArchiveFolder, fName);
                        if (File.Exists(targetPath))
                        {
                            targetPath = string.Format("{0}{1}.{2}",

                                Path.GetFileNameWithoutExtension(targetPath),
                                "ext",
                                 Path.GetExtension(targetPath));
                        }
                        File.Move(file, targetPath);
                        Log.Info("File [{0}] processing success", fName);

                        //3. Notify consumer
                        notifyConsumer(setting.Source);
                    }
                    catch(Exception ex)
                    {
                        Log.Error(string.Format("Error when process [{0}]:\r\n{1}\r\n{2}",
                            file, ex.Message,
                            ex.Source));
                    }
                    
                }

            }
        }

        private void bulkCopy(string filePath, string dataLoadTableName)
        {
            var connection = DA.Connection;
            var sr = new StreamReader(filePath);

            string line = sr.ReadLine();

            if (line == null || line.Trim() == string.Empty)
            {
                sr.Close();
                return;
            }

            var dt = new DataTable();
            string[] strArray = line.Split('|');

            for (int index = 0; index < strArray.Length; index++)
            {
                dt.Columns.Add(new DataColumn());
            }

            do
            {
                DataRow row = dt.NewRow();
                string[] itemArray = line.Split('|');
                string[] decodedArray = new string[itemArray.Length];
                for (int i = 0; i < itemArray.Length; i++)
                {
                    decodedArray[i] = HttpUtility.UrlDecode(itemArray[i]);
                }
                row.ItemArray = decodedArray;
                dt.Rows.Add(row);
                line = sr.ReadLine();
            } while (!string.IsNullOrEmpty(line));


            var bc = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, null)
            {
                DestinationTableName = dataLoadTableName,
                BatchSize = dt.Rows.Count
            };
            connection.Open();
            bc.WriteToServer(dt);
            connection.Close();
            bc.Close();
            sr.Close();
        }

        private void notifyConsumer(string source)
        {
            var consumerUrl = Configuration.GetApp("consumerUrl");
            var client = new HttpClient();
            var url = new Uri(string.Format(consumerUrl, source));
            var responseStatus = client.GetAsync(url).Result.StatusCode;
            Log.Info("Consumer API - URL[{0}] , Response Status [{1}]", url, responseStatus);
        }
    }
}
