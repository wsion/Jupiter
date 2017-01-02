using System.Data;


namespace Jupiter.Utility
{
    public class ConnectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType">mssql,mysql,oracle</param>
        /// <param name="conStr"></param>
        /// <returns></returns>
        public static IDbConnection CreateConnection(string dbType, string conStr)
        {
            IDbConnection con;
            switch (dbType.ToLower())
            {
                case "mssql":
                    con = new System.Data.SqlClient.SqlConnection(conStr);
                    break;
                case "mysql":
                    con = new MySql.Data.MySqlClient.MySqlConnection(conStr);
                    break;
                case "oracle":
                    con = new Oracle.DataAccess.Client.OracleConnection(conStr);
                    break;
                default:
                    con = null;
                    break;
            }

            return con;
        }
    }
}
