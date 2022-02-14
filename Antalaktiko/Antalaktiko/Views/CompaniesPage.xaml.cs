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
	public partial class CompaniesPage : ContentPage
	{
        private CompaniesViewModel _viewModel;

        public CompaniesPage ()
		{
			InitializeComponent ();
			BindingContext = _viewModel = new CompaniesViewModel();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private void OpenPopUp_Clicked(object sender, EventArgs e)
        {
            filterpopup.IsOpen = true;
        }

        private void ClosePopup_Clicked(object sender, EventArgs e)
        {
            filterpopup.IsOpen = false;
        }
    }
}