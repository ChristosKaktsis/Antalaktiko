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
        protected override  void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        private void SearchText_TextChanged(object sender, EventArgs e)
        {
            BrandCollectionView.FilterString = "Contains([Name], '" + SearchText.Text + "')";
        }

        private void SearchPartText_TextChanged(object sender, EventArgs e)
        {
            PartsCollectionView.FilterString = "Contains([Title], '" + SearchPartText.Text + "')";
        }
        private void SearchRegionText_TextChanged(object sender, EventArgs e)
        {
            RegionCollection.FilterString = "Contains([Title], '" + SearchRegionText.Text + "')";
        }

        private void OpenAfmPopup_Clicked(object sender, EventArgs e)
        {
            afmpopup.IsOpen = true;
        }

        private void CloseAfmPopUp_Clicked(object sender, EventArgs e)
        {
            afmpopup.IsOpen = false;
        }

        private async void Privacy_Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new ContactPage("https://www.antalaktiko.gr/privacy-policy/"));
        }

        private async void Terms_Button_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new ContactPage("https://www.antalaktiko.gr/terms_of_use/"));
        }

        private void Region_Popup_Clicked(object sender, EventArgs e)
        {
            RegionPopUp.IsOpen = !RegionPopUp.IsOpen;
        }
    }
}