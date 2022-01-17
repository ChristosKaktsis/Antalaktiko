using Antalaktiko.Data;
using Antalaktiko.Services;
using Antalaktiko.Views;
using System;
using System.IO;
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
        public App()
        {
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.CollectionView.Initializer.Init();
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();          
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
