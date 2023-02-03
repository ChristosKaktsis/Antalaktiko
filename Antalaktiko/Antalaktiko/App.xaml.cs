using Antalaktiko.Data;
using Antalaktiko.Models;
using Antalaktiko.Services;
using Antalaktiko.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Antalaktiko
{
    public partial class App : Application
    {
        static Database database;

        // Create the database connection as a singleton.
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tk.db3"));
                }
                return database;
            }
        }

        public static User LogedUser { get; set; }

        public static bool IsLoggedIn
        {
            get => Preferences.Get(nameof(IsLoggedIn), false);
            set
            {
                Preferences.Set(nameof(IsLoggedIn), value);
            }
        }
        public App()
        {
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            DevExpress.XamarinForms.Popup.Initializer.Init();
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
            if (!IsLoggedIn)
                GoToLogInPage();
            else
                CheckUser();
        }

        private async void CheckUser()
        {
            try
            {
                var username = Preferences.Get("UserMail", string.Empty);
                var password = Preferences.Get("Password", string.Empty);
                LogedUser = await GetUser(username, password);
                if (LogedUser.Error != 0)
                    GoToLogInPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           
        }

        public static async Task<User> GetUser(string email, string password)
        {

            var login = new { type ="login", email, password };
            UserManager userManager = new UserManager();
            //var user = await userManager.LogIn(login);
            var user = await userManager.Login(email, password);
            return user;
        }
       

        private async void GoToLogInPage()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
