using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace Jupiter.Utility
{
    public class DataAccess
    {
        private SqlConnection con;

        public SqlConnection Connection
        {
            get
            {
                return con;
            }
        }

        public DataAccess(string conName)
        {
            var conStr = Jupiter.Utility.Configuration.GetConnection("connection");
            con = this.GenerateConnection(conStr);
        }

        public SqlConnection GenerateConnection(string conStr)
        {
            return new SqlConnection(conStr);
        }

        public IEnumerable<T> Query<T>(string cmd)
        {
            con.Open();
            var result = con.Query<T>(cmd);
            con.Close();
            return result;
        }

        public int Execute(string sql)
        {
            con.Open();
            var resut = con.Execute(sql);
            con.Close();
            return resut;
        }
        public int Execute(string sql, object paras)
        {
            con.Open();
            var resut = con.Execute(sql, paras, commandType: System.Data.CommandType.StoredProcedure);
            con.Close();
            return resut;
        }
    }
}
