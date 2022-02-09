using Antalaktiko.Views;
using System;
using System.Collections.Generic;
using System.Text;
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
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(async ()=> await Shell.Current.GoToAsync(nameof(RegisterPage)));
            ContactCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ContactPage)));
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
                var login = new { username = UserMail, password = Password };
                var user = await userManger.LogIn(login);
                var errorCheck = CheckError(user.Error);
                if (errorCheck)
                    return;
                App.LogedUser = user;
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
