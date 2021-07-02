using Acr.UserDialogs;
using EcommerceApp.Pages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcommerceApp.ViewModels
{
    public class LoginViewModel
    {
        #region [Constructor]
        public LoginViewModel(INavigation navegation)
        {
            _inavigation = navegation;
            SignInCommand = new Command(async () => await SignIn());
        }

        #endregion
        #region [Private attributes]
        private string _email;
        private string _password;
        private INavigation _inavigation { get; set; }
        #endregion

        #region [Public attributes]
        public ICommand SignInCommand { get; set; }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        #endregion

        #region [Methods]
        private async Task SignIn()
        {
            if (Password == string.Empty || Email == string.Empty)
            {
                await UserDialogs.Instance.AlertAsync("Please check, Passwod is empty", "Password Empty");
            }
            else
            {
                var signIn = new
                {
                    Email = Email,
                    Password = Password
                };

                string json = JsonConvert.SerializeObject(signIn);
                HttpClient httpClient = new HttpClient();
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("https://ecommercebackendapi.azurewebsites.net/api/Accounts/Login", content);

                

                if (!response.IsSuccessStatusCode)
                {
                    await UserDialogs.Instance.AlertAsync("Please, Try again later", "Error");
                }
                else
                {
                    string c = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject(c);
                    await UserDialogs.Instance.AlertAsync(resultado.ToString(), "Welcome");
                    //await _inavigation.PushModalAsync(new SignUpPage());
                }
            }
        }
        #endregion
    }
}
