using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace OnlineStoreDB.Model
{
    public class Order_Items : INotifyPropertyChanged
    {
        private int orderItemId;
        private int order_ID;
        private int product_ID;
        private int quantity;
        [Key]
        public int OrderItemID
        {
            get { return orderItemId; }
            set
            {
                orderItemId = value;
                OnPropertyChanged(nameof(OrderItemID));
            }
        }
        public int Order_ID
        {
            get { return order_ID; }
            set
            {
                order_ID = value;
                OnPropertyChanged(nameof(Order_ID));
            }
        }

        public int Product_ID
        {
            get { return product_ID; }
            set
            {
                product_ID = value;
                OnPropertyChanged(nameof(Product_ID));
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
  
}
