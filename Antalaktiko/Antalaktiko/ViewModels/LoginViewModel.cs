using Antalaktiko.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(async ()=> await Shell.Current.GoToAsync(nameof(RegisterPage)));
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
        }
    }
}
