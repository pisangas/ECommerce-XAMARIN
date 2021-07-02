using Acr.UserDialogs;
using EcommerceApp.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcommerceApp.ViewModel
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        #region Constructors
        public SignUpViewModel(INavigation navigation)
        {
            _navegation = navigation;
            ContinueCommand = new Command(async () => await Continue());
            LoginCommand = new Command(async () => await Login());
        }
        #endregion

        #region Private Properties
        private INavigation _navegation;
        private string _fullName;
        private string _email;
        private string _password;
        private string _confirmPassword;

        #endregion

        #region Public Properties
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ContinueCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        public string Fullname
        {
            get { return _fullName; }
            set { _fullName = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Password
        {
            get { return _password; }
            set { 
                _password = value;
                NotifyPropertyChanged();            
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { 
                _confirmPassword = value;
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Methods
        private async Task Continue()
        {
            if (Password != ConfirmPassword)
            {
                Password = string.Empty;
                ConfirmPassword = string.Empty;
                await UserDialogs.Instance.AlertAsync("Plase, check your password", "Password mismatch");
            }
            else
            {
                var register = new
                {
                    Name = Fullname,
                    Email = Email,
                    Password = ConfirmPassword
                };

                var json = JsonConvert.SerializeObject(register);
                var httpClient = new HttpClient();
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://ecommercebackendapi.azurewebsites.net/api/Accounts/Register", content);

                if (!response.IsSuccessStatusCode)
                {
                    await UserDialogs.Instance.AlertAsync("Please, try again later", "Error");

                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Your account has been cretaed", "Hi");
                    await _navegation.PushAsync(new LoginPage());
                }
            }

        }
        private async Task Login()
        {
            await _navegation.PushAsync(new LoginPage());
        }
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
