using Antalaktiko.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Antalaktiko.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private RegisterViewModel _viewModel;

        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new RegisterViewModel();
        }
    }
}