using Antalaktiko.ViewModels;
using Antalaktiko.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Antalaktiko
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(NewAdPage), typeof(NewAdPage));
        }

    }
}
