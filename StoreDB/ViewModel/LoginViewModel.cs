using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows;
using System.Windows.Input;
using OnlineStoreDB;
using OnlineStoreDB.Model;
using OnlineStoreDB.View;


namespace OnlineStoreDB.ViewModel
{
    public class LoginViewModel: INotifyPropertyChanged
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public ICommand OpenSignUpCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel() 
        {
            OpenSignUpCommand = new RelayCommand(OpenSignUp);
            LoginCommand = new RelayCommand(Login);
            OpenGuestCommand = new RelayCommand(OpenGuest);
        }

        private void OpenSignUp(object obj)
        {
            
            SignUpWindow signUpWindow = new SignUpWindow(); 
            Application.Current.MainWindow.Close();
            signUpWindow.Show();
        }

        private void Login(object obj)
        {
            try
            {
                
                Users user;
                using (var dbContext = ApplicationContextDB.getInstance())
                {
                    user = dbContext.Users.FirstOrDefault(u => u.UserName == this.UserName);
                }

                
                if (user != null && user.UserPassword == this.UserPassword)
                {

                    
                    if (user.UserRole == "admin")
                    {
                        AdminWindow adminWindow = new AdminWindow();
                        adminWindow.Show();
                    }
                    else if (user.UserRole == "employee")
                    {
                        EmployeeWindow employeeWindow = new EmployeeWindow();
                        employeeWindow.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while logging in: " + ex.Message);
            }
            Application.Current.MainWindow.Close();
        }


        public ICommand OpenGuestCommand { get; set; }
        private void OpenGuest(object obj)
        {

            GuestWindow guestwindow = new GuestWindow();
            Application.Current.MainWindow.Close();
            guestwindow.Show();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}


