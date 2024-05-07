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
    public class Offices : INotifyPropertyChanged
    {
        private int officeId;
        private string address;
        private string city;
        private string officeHours;
        private int rating;
        private int ordersVolume;
        [Key]
        public int OfficeID
        {
            get { return officeId; }
            set
            {
                officeId = value;
                OnPropertyChanged(nameof(OfficeID));
            }
        }
        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public string Office_Hours
        {
            get { return officeHours; }
            set
            {
                officeHours = value;
                OnPropertyChanged(nameof(Office_Hours));
            }
        }

        public int Rating
        {
            get { return rating; }
            set
            {
                rating = value;
                OnPropertyChanged(nameof(Rating));
            }
        }

        public int Orders_Volume
        {
            get { return ordersVolume; }
            set
            {
                ordersVolume = value;
                OnPropertyChanged(nameof(Orders_Volume));
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
