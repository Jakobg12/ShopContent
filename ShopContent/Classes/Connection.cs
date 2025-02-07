using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ShopContent.Classes
{
    public class Connection
    {
        private static readonly string config = "server=DESKTOP-PM8TUKA;Trusted_Connection=No;database=ShopContent;User Id=sa;Password=root;";
        public static SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(config);
            connection.Open();
            return connection;
        }

        public static SqlDataReader Query(string SQL, out SqlConnection connection)
        {
            connection = OpenConnection();
            return new SqlCommand(SQL, connection).ExecuteReader();
        }

        public static void CloseConnection(SqlConnection connection)
        {
            connection.Close();
            SqlConnection.ClearPool(connection);
        }
    }
}