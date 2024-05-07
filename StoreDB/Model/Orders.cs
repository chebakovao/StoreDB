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
    public class Orders : INotifyPropertyChanged
    {
        private int orderId;
        private DateTime orderDate;
        private decimal orderPrice;
        private string orderStatus;
        private int customerId;
        private string employee;
        private int orderOfficeId;
        private DateTime pickupDate;
        [Key]
        public int OrderID
        {
            get { return orderId; }
            set
            {
                orderId = value;
                OnPropertyChanged(nameof(OrderID));
            }
        }
        public DateTime Order_Date
        {
            get { return orderDate; }
            set
            {
                orderDate = value;
                OnPropertyChanged(nameof(Order_Date));
            }
        }

        public decimal Order_Price
        {
            get { return orderPrice; }
            set
            {
                orderPrice = value;
                OnPropertyChanged(nameof(Order_Price));
            }
        }

        public string Order_Status
        {
            get { return orderStatus; }
            set
            {
                orderStatus = value;
                OnPropertyChanged(nameof(Order_Status));
            }
        }

        public int CustomerId
        {
            get { return customerId; }
            set
            {
                customerId = value;
                OnPropertyChanged(nameof(CustomerId));
            }
        }

        public string Employee
        {
            get { return employee; }
            set
            {
                employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }

        public int OrderOfficeId
        {
            get { return orderOfficeId; }
            set
            {
                orderOfficeId = value;
                OnPropertyChanged(nameof(OrderOfficeId));
            }
        }

        public DateTime Pickup_Date
        {
            get { return pickupDate; }
            set
            {
                pickupDate = value;
                OnPropertyChanged(nameof(Pickup_Date));
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
