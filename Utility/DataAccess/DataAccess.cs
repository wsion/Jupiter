using System.Data;
using System;

namespace Jupiter.Utility
{
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
    }
}
