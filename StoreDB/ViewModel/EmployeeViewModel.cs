using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
using LiveCharts.Wpf;
using LiveCharts;
using Microsoft.EntityFrameworkCore;
using OnlineStoreDB.Model;

namespace OnlineStoreDB.ViewModel
{
    public class EmployeeViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<Orders> _orders;
        public ObservableCollection<Orders> Orders
        {
            get { return _orders; }
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }


        public EmployeeViewModel()
        {
            LoadOrders();
            FilterOrders();
            MarkAsDoneCommand = new RelayCommand(MarkAsDone, CanMarkAsDone);
            OrderOffices = new ObservableCollection<int>(GetColumnData());
        }

        private void LoadOrders()
        {
            using (var context = new ApplicationContextDB())
            {
                Orders = new ObservableCollection<Orders>(context.Orders.ToList());
            }
        }

        public ICommand MarkAsDoneCommand { get; set; }


        private void MarkAsDone(object obj)
        {
            if (SelectedOrder != null)
            {
                SelectedOrder.Order_Status = "Completed";
                using (var context = new ApplicationContextDB())
                {
                    
                    context.Orders.Attach(SelectedOrder);
                    
                    context.Entry(SelectedOrder).State = EntityState.Modified;
                   
                    context.SaveChanges();
                }
            }
        }

        private bool CanMarkAsDone(object obj)
        {
            
            return SelectedOrder != null;
        }

      
        private Orders _selectedOrder;
        public Orders SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));

            }
        }



        private string _searchQuery;
        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                FilterOrders();
            }
        }

        private ObservableCollection<Orders> _filteredOrders;
        public ObservableCollection<Orders> FilteredOrders
        {
            get { return _filteredOrders; }
            set
            {
                _filteredOrders = value;
                OnPropertyChanged(nameof(FilteredOrders));
            }
        }

        private void FilterOrders()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
              
                FilteredOrders = FilteredOrdersbyOffice;
            }
            else
            {
 
                FilteredOrders = new ObservableCollection<Orders>(
                    FilteredOrdersbyOffice.Where(order =>
                        order.Order_Status.ToLower().Contains(SearchQuery.ToLower()) ||
                        order.Employee.ToLower().Contains(SearchQuery.ToLower()) ||
                        order.OrderID.ToString().Contains(SearchQuery.ToLower())
                    )
                );
            }
        }



        private List<int> GetColumnData()
        {
            using (var dbContext = new ApplicationContextDB())
            {

                return dbContext.Orders.Select(row => row.OrderOfficeId).Distinct().ToList();
            }
        }
        private ObservableCollection<int> orderOffices;
        public ObservableCollection<int> OrderOffices
        {
            get { return orderOffices; }
            set
            {
                orderOffices = value;
                OnPropertyChanged(nameof(OrderOffices));
            }
        }

        private int _selectedOffice;
        public int SelectedOffice
        {
            get { return _selectedOffice; }
            set
            {
                _selectedOffice = value;
                OnPropertyChanged(nameof(SelectedOffice));

                FilterOrdersByOffice(_selectedOffice);
                FilterOrders();
                UpdateOrdersCountByPickupDate();
            }
        }


        private ObservableCollection<Orders> _filteredOrdersbyOffice;
        public ObservableCollection<Orders> FilteredOrdersbyOffice
        {
            get { return _filteredOrdersbyOffice; }
            set
            {
                _filteredOrdersbyOffice = value;
                OnPropertyChanged(nameof(FilteredOrdersbyOffice));
            }
        }

        private void FilterOrdersByOffice(int selectedOfficeId)
        {
            if (selectedOfficeId == 0)
            {
                FilteredOrdersbyOffice = Orders; 
            }
            else
            {
                FilteredOrdersbyOffice = new ObservableCollection<Orders>(
                    Orders.Where(order => order.OrderOfficeId == selectedOfficeId));
            }
        }


        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }

        private ObservableCollection<string> _labels;
        public ObservableCollection<string> Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }

        private void UpdateOrdersCountByPickupDate()
        {
           
                var orders = FilteredOrdersbyOffice.ToList();

               
                var orderCounts = orders.GroupBy(o => o.Order_Date.Date)
                                        .Select(group => new { Date = group.Key, Count = group.Count() })
                                        .OrderBy(item => item.Date)
                                        .ToList();

                
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Amount of Orders",
                        Values = new ChartValues<int>(orderCounts.Select(item => item.Count)),
                    }
                };

                Labels = new ObservableCollection<string>(orderCounts.Select(item => item.Date.ToShortDateString()));
            
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
