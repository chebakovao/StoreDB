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
    public class Employees : INotifyPropertyChanged
    {
        private int employeeId;
        private string name;
        private int position_ID;
        private int seller_ID;
        [Key]
        public int EmployeeID
        {
            get { return employeeId; }
            set
            {
                employeeId = value;
                OnPropertyChanged(nameof(EmployeeID));
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int Position_ID
        {
            get { return position_ID; }
            set
            {
                position_ID = value;
                OnPropertyChanged(nameof(Position_ID));
            }
        }

        public int Seller_ID
        {
            get { return seller_ID; }
            set
            {
                seller_ID = value;
                OnPropertyChanged(nameof(Seller_ID));
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
