using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace Jupiter.Utility
{
    /// <summary>
    /// Dapper based SqlDataAccess
    /// </summary>
    public class SqlDataAccess
    {
        private SqlConnection con;

        public SqlConnection Connection
        {
            get
            {
                return con;
            }
        }

        public SqlDataAccess(string conName)
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

        /// <summary>
        ///  Executes a query, returning the data typed as per T 
        ///  <para></para>
        ///  Example: Query &lt;SomeType&gt; ("SELECT * FROM T1 WHERE id = @id", new { id = 1})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">sql with parameters</param>
        /// <param name="paras">object contains parameters</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string cmd, object paras)
        {
            con.Open();
            var result = con.Query<T>(cmd, paras);
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

        public IDataReader ExecuteReader(string sql)
        {
            con.Open();
            var result = con.ExecuteReader(sql);
            con.Close();
            return result;
        }

        /// <summary>
        ///  Execute parameterized SQL and execute an callback for each record
        ///  <para></para>
        ///  Example: ExecuteReader("SELECT * FROM T1 WHERE id = @id", new { id = 1}, (record)=>{ })
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd">sql with parameters</param>
        /// <param name="paras">object contains parameters</param>
        /// <param name="callback">callback which processes IDataRecord</param>
        public void ExecuteReader(string sql, object paras, Action<IDataRecord> callback)
        {
            con.Open();
            var reader = con.ExecuteReader(sql, paras);
            while (reader.Read())
            {
                callback.Invoke((IDataRecord)reader);
            }
            reader.Close();
            con.Close();
        }
    }
}
