using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace Dev
{
    class SQLParameters
    {
        public void start()
        {
            var list = new ArrayList();
            list.Add(new string[] { "101", "TRUE", "13996123456" });
            list.Add(new string[] { "102", "TRUE", "13996123456" });
            list.Add(new string[] { "103", "TRUE", "13996123456" });
            list.Add(new string[] { "104", "TRUE", "13996123456" });


            var columns = new[] { "memcardno", "sex", "handset" };

            SqlCommand cmd = new SqlCommand();
            cmd.Connection =
                new SqlConnection(
                    "Data Source=.;Initial Catalog=Jupiter;Integrated Security=True");

            string insertion = "IF NOT EXISTS " +
                "(SELECT TOP 1 1 FROM Dummy_Membership WHERE [memcardno]=@memcardno)" +
                "INSERT INTO [Dummy_Membership] ([memcardno],[sex],[handset]) " +
                "VALUES (@memcardno,@sex,@handset);" +
                "UPDATE Dummy_Membership SET createtime = getdate() WHERE " +
                "[memcardno]=@memcardno; ";

            cmd.CommandText = insertion;

            foreach (var col in columns)
            {
                cmd.Parameters.Add("@" + col, SqlDbType.NVarChar);
            }

            cmd.Connection.Open();

            foreach (string[] obj in list)
            {
                for (var i = 0; i < columns.Length; i++)
                {
                    cmd.Parameters[i].Value = obj[i];
                }

                cmd.ExecuteNonQuery();
            }

            cmd.Connection.Close();

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
