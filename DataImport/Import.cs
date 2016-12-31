﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Jupiter.Utility;

namespace DataImport
{
    class Import
    {
        private DataAccess DA = new DataAccess("connection");

        private string TableName = "DataLoad";

        public void Start()
        {
            var settings = DA.Query<Jupiter.DataModel.ImportSetting>("SELECT [Source],[TableName],[Columns],[Directory],[FilePattern] FROM [ImportSetting]");

            foreach (var setting in settings)
            {
                var files = Directory.GetFiles(setting.Directory, setting.FilePattern);

                foreach (var file in files)
                {
                    bulkCopy(file);

                    //Process data
                    DA.Execute("[SP_Import]", new { tableName = setting.TableName, columns = setting.Columns, source = setting.Source, fileName = Path.GetFileName(file) });

                    //Todo:Archive file
                }

            }
        }

        private void bulkCopy(string filePath)
        {
            var connection = DA.Connection;
            var sr = new StreamReader(filePath);

            string line = sr.ReadLine();

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
                row.ItemArray = itemArray;
                dt.Rows.Add(row);
                line = sr.ReadLine();
            } while (!string.IsNullOrEmpty(line));


            var bc = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, null)
            {
                DestinationTableName = this.TableName,
                BatchSize = dt.Rows.Count
            };
            connection.Open();
            bc.WriteToServer(dt);
            connection.Close();
            bc.Close();
        }
    }
}
