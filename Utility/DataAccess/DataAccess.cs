using System.Data;
using System;
using System.Collections.Generic;

namespace Jupiter.Utility
{
    /// <summary>
    /// Used to walk through selection result set for different DB manufacturer
    /// including MS-SQL, MySql, Oracle.
    /// </summary>
    public class DataAccess
    {
        private IDbConnection con;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType">mssql,mysql,oracle</param>
        /// <param name="conStr"></param>
        public DataAccess(string dbType, string conStr)
        {
            con = ConnectionFactory.CreateConnection(dbType, conStr);
        }

        public int LoopSelectResult(string query, Action<string, bool> loopAction = null, Action newLineAction = null)
        {
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = query;
            var reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                count++;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    loopAction?.Invoke(reader[i].ToString(), i == reader.FieldCount - 1);
                }

                newLineAction?.Invoke();
            }
            con.Close();
            return count;
        }

        /// <summary>
        /// Excute command text with parameters.
        /// </summary>
        /// <param name="commandText">
        /// 
        /// </param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, List<KeyValuePair<string, string>> paras)
        {
            using (IDbCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = commandText;
                con.Open();

                foreach (var para in paras)
                {
                    var parameter = cmd.CreateParameter();
                    parameter.DbType = DbType.String;
                    parameter.ParameterName = para.Key;
                    parameter.Value = para.Value;
                    cmd.Parameters.Add(parameter);
                }
                int affectedRows = cmd.ExecuteNonQuery();
                con.Close();
                return affectedRows;
            }
        }
    }
}
