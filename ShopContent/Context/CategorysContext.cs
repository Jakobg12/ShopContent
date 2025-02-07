using ShopContent.Classes;
using ShopContent.Modell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;

namespace ShopContent.Context
{
    public class CategoriesContext : Categories
    {
        public static ObservableCollection<CategoriesContext> AllCategories()
        {
            ObservableCollection<CategoriesContext> allCategories = new ObservableCollection<CategoriesContext>();
            SqlConnection connection;
            SqlDataReader dataCategories = Connection.Query("SELECT * FROM [dbo].[Categories]", out connection);
            while (dataCategories.Read())
            {
                allCategories.Add(new CategoriesContext()
                {
                    Id = dataCategories.GetInt32(0),
                    Name = dataCategories.GetString(1)
                });
            }
            Connection.CloseConnection(connection);
            return allCategories;
        }
    }

}
