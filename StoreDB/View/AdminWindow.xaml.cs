using OnlineStoreDB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OnlineStoreDB.View
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            DataContext = new AdminViewModel();
            InitializeComponent();
        }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is AdminViewModel viewModel)
            {
                viewModel.selectedRows = ((DataGrid)sender).SelectedItems.Cast<object>().ToList();
            }
        }
    }
}
