using OnlineStoreDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreDB.ViewModel
{
    public class GuestViewModel: INotifyPropertyChanged
    {
        private string searchQuery;
        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                if (searchQuery != value)
                {
                    searchQuery = value;
                    OnPropertyChanged();
                    FilterProducts();
                }
            }
        }

        private ObservableCollection<Products> filteredProducts;
        public ObservableCollection<Products> FilteredProducts
        {
            get { return filteredProducts; }
            set
            {
                filteredProducts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Products> products;
        public ObservableCollection<Products> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged();
            }
        }


        public GuestViewModel()
        {
            LoadOrders();
            FilterProducts();
        }

        private void LoadOrders()
        {
            using (var context = new ApplicationContextDB())
            {
                Products = new ObservableCollection<Products>(context.Products.ToList());
            }
        }

        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredProducts = Products;
            }
            else
            {
                FilteredProducts = new ObservableCollection<Products>(
                Products.Where(p => p.Name.ToLower().Contains(SearchQuery.ToLower()) ||
                                p.ProductID.ToString().Contains(SearchQuery.ToLower()) ||
                                p.Category.ToLower().Contains(SearchQuery.ToLower()) ||
                                p.Description.ToLower().Contains(SearchQuery.ToLower())).ToList());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
