using ShopContent.Classes;
using ShopContent.Modell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ShopContent.Context
{
    public class ItemsContext : Items
    {
        public ItemsContext(bool save = false)
        {
            if (save)
                Save(true);
            Category = new Categories();
        }

        public static ObservableCollection<ItemsContext> AllItems()
        {
            ObservableCollection<ItemsContext> allItems = new ObservableCollection<ItemsContext>();
            ObservableCollection<CategoriesContext> allCategories = CategoriesContext.AllCategories();
            SqlConnection connection;
            SqlDataReader dataItems = Connection.Query("SELECT * FROM [dbo].[Items]", out connection);
            while (dataItems.Read())
            {
                allItems.Add(new ItemsContext()
                {
                    Id = dataItems.GetInt32(0),
                    Name = dataItems.GetString(1),
                    Price = dataItems.GetDouble(2),
                    Description = dataItems.GetString(3),
                    Category = dataItems.IsDBNull(4) ?
                    null :
                    allCategories.Where(x => x.Id == dataItems.GetInt32(4)).First()
                });
            }
            Connection.CloseConnection(connection);
            return allItems;
        }

        public void Save(bool New = false)
        {
            SqlConnection connection = null;
            if (New)
            {
                SqlDataReader dataItems = Connection.Query($"INSERT INTO [dbo].[Items](Name, Price, Description) OUTPUT Inserted.Id VALUES (N'{this.Name}' , {this.Price}, N'{this.Description}')", out connection);
                dataItems.Read();
                this.Id = dataItems.GetInt32(0);
            }
            else
            {
                Connection.Query("UPDATE [dbo].[Items] SET " +
                                 $"Name  = N'{this.Name}', Price = {this.Price}, Description = N'{this.Description}', IdCategory = {this.Category.Id} WHERE Id = {this.Id}", out connection);
            }
            Connection.CloseConnection(connection);
            MainWindow.init.frame.Navigate(MainWindow.init.Main);
        }

        public void Delete()
        {
            SqlConnection connection;
            Connection.Query($"DELETE FROM [dbo].[Items] WHERE id = {this.Id}", out connection);
            Connection.CloseConnection(connection);
        }

        public RelayCommand OnEdit
        {
            get
            {
                return new RelayCommand(obj => { MainWindow.init.frame.Navigate(new View.Add(this)); });
            }
        }

        public RelayCommand OnSave
        {
            get
            {
                return new RelayCommand(obj =>
                {
                    Category = CategoriesContext.AllCategories().Where(x => x.Id == this.Category.Id).First();
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
                    (MainWindow.init.Main.DataContext as ViewModell.VMItems).Items.Remove(this);
                });
            }
        }
    }
}