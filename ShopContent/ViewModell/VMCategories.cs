using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ShopContent.ViewModell
{
    public class VMCategories : INotifyPropertyChanged
    {
        public ObservableCollection<Context.CategoriesContext> Categories { get; set; }
        public VMCategories() => Categories = Context.CategoriesContext.AllCategories();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
