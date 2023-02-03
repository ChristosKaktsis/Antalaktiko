using Antalaktiko.Models;
using Antalaktiko.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string usermail;
        private string password;
        private string errorMessage;
        
        private bool emailhasError;
        private bool passhasError;

        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command ContactCommand { get; }
        public LoginViewModel()
        {
            App.IsLoggedIn = false;
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(async ()=> await Shell.Current.GoToAsync(nameof(RegisterPage)));
            ContactCommand = new Command(async () => await Shell.Current.Navigation.PushAsync(new ContactPage("https://www.antalaktiko.gr/%ce%b5%cf%80%ce%b9%ce%ba%ce%bf%ce%b9%ce%bd%cf%89%ce%bd%ce%af%ce%b1/")));
        }
        public string UserMail
        {
            get => Preferences.Get(nameof(UserMail), string.Empty);
            set
            {
                Preferences.Set(nameof(UserMail), value);
                SetProperty(ref usermail, value);
            }
        }
        public string Password
        {
            get => Preferences.Get(nameof(Password),string.Empty);
            set
            {
                Preferences.Set(nameof(Password), value);
                SetProperty(ref password, value);
            }
        }
        private async void OnLoginClicked()
        {
            try
            {
                IsBusy = true;
                var user = await App.GetUser(UserMail,Password);
                if (CheckError(user.Error))
                    return;
                App.LogedUser = user;
                App.IsLoggedIn = true;
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(StartPage)}");
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
           
        }

        

        public string ErrorMessage 
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value); 
        }
        public bool EmailHasError
        {
            get => emailhasError;
            set => SetProperty(ref emailhasError, value);
        }
        public bool PassHasError
        {
            get => passhasError;
            set => SetProperty(ref passhasError, value);
        }
        private bool CheckError(int error)
        {
            EmailHasError = false;
            PassHasError = false;
            switch (error)
            {
                
                case 1:
                    ErrorMessage = "Λάθος Email";
                    EmailHasError = true;
                    break;
                case 2:
                    ErrorMessage = "Λάθος Κωδικός";
                    PassHasError = true;
                    break;
                case 3:
                    ErrorMessage = "Ο χρήστης δεν έχει πάρει έγκριση";
                    EmailHasError = true;
                    break;
                case 4:
                    ErrorMessage = "Ο χρήστης είναι ληγμένος";
                    EmailHasError = true;
                    break;
                case 5:
                    ErrorMessage = "Ο χρήστης είναι παλιός";
                    EmailHasError = true;
                    break;
            }
            return EmailHasError || PassHasError;
        }
    }
}
