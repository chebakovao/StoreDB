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
using OnlineStoreDB.Model;
using OnlineStoreDB.ViewModel;


namespace OnlineStoreDB.View
{
 
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            
            DataContext = new SignUpViewModel();
            InitializeComponent();
        }
    }
}
