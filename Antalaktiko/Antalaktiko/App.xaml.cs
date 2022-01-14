using Antalaktiko.Services;
using Antalaktiko.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Antalaktiko
{
    public partial class App : Application
    {

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
