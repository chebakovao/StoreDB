using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using OnlineStoreDB.Model;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using OnlineStoreDB.View;

namespace OnlineStoreDB.ViewModel
{
    public class SignUpViewModel : INotifyPropertyChanged
    {

        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserRole { get; set; }
        public List<string> AvailableRoles { get; set; }
        public ObservableCollection<Users> Users { get; set; }


        public ICommand SignUpCommand { get; private set; }

        public SignUpViewModel() 
        {
            AvailableRoles = new List<string> { "admin", "employee" };
            SignUpCommand = new RelayCommand(SignUp);
        }

        private void SignUp()
        {
            try
            {
                
                Users newUser = new Users
                {
                   
                    UserName = UserName,
                    UserPassword = UserPassword,
                    UserRole = UserRole
                };

                
                ApplicationContextDB dbContext = ApplicationContextDB.getInstance();

                
                dbContext.Users.Add(newUser);

                
                dbContext.SaveChanges();
                OpenLogin();
                
                
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Error occurred: " + ex.Message);
                
            }
        }

        public class RelayCommand : ICommand
        {
            private Action _action;

            public RelayCommand(Action action)
            {
                _action = action;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                _action();
            }
        }

        private void OpenLogin()
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
