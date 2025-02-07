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
        public CategoriesContext(bool save = false)
        {
            if (save)
                Save(true);
        }

        public static ObservableCollection<CategoriesContext> AllCategories()
        {
            ObservableCollection<CategoriesContext> allCategories = new ObservableCollection<CategoriesContext>();
            SqlConnection connection;
            SqlDataReader dataCategories = Connection.Query("SELECT * FROM [dbo].[Categories]", out connection);
            while (dataCategories.Read())
            {
                allCategories.Add(new CategoriesContext() { Id = dataCategories.GetInt32(0), Name = dataCategories.GetString(1) });
            }
            Connection.CloseConnection(connection);
            return allCategories;
        }

        public void Save(bool New = false)
        {
            SqlConnection connection = null;
            if (New)
            {
                SqlDataReader dataItems = Connection.Query($"INSERT INTO [dbo].[Categories](Name) OUTPUT Inserted.Id VALUES (N'{this.Name}')", out connection);
                dataItems.Read();
                this.Id = dataItems.GetInt32(0);
            }
            else
            {
                Connection.Query("UPDATE [dbo].[Categories] SET " +
                                 $"Name  = N'{this.Name}' WHERE Id = {this.Id}", out connection);
            }

            Connection.CloseConnection(connection);
            MainWindow.Instance.frame.Navigate(MainWindow.Instance.MainCategories);
        }

        public void Delete()
        {
            SqlConnection connection;
            Connection.Query($"DELETE FROM [dbo].[Categories] WHERE id = {this.Id}", out connection);
            Connection.CloseConnection(connection);
        }

        public RelayCommand OnEdit
        {
            get
            {
                return new RelayCommand(obj => { MainWindow.Instance.frame.Navigate(new View.Categories.Add(this)); });
            }
        }

        public RelayCommand OnSave
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Save();
                });
            }
        }

        public RelayCommand OnDelete
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Delete();
                    (MainWindow.Instance.MainCategories.DataContext as ViewModell.VMCategories).Categories.Remove(this);
                });
            }
        }
    }

}
