using Antalaktiko.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string usermail;
        private string password;

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command ContactCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(async ()=> await Shell.Current.GoToAsync(nameof(RegisterPage)));
            ContactCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ContactPage)));
        }
        public string UserMail
        {
            get => usermail;
            set
            {
                SetProperty(ref usermail, value);

            }
        }
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);

            }
        }
        private async void OnLoginClicked(object obj)
        {
            var login = new { UserMail, Password };
           // await userManger.LogIn(login);
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
        }
    }
}
