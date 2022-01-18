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
    public partial class NewAdPage : ContentPage
    {
        private NewAdViewModel _viewModel;

        public NewAdPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewAdViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            brandspopup.IsOpen = true;
        }

        private void ClosePopup_Clicked(object sender, EventArgs e)
        {
            brandspopup.IsOpen = false;
        }
        private void SearchText_TextChanged(object sender, EventArgs e)
        {
            BrandCollectionView.FilterString = "Contains([Name], '" + SearchText.Text + "')";
        }

        private void Models_Button_Clicked(object sender, EventArgs e)
        {
            modelspopup.IsOpen = true;
        }
        private void CloseModelsPopUp_Clicked(object sender, EventArgs e)
        {
            modelspopup.IsOpen = false;
        }
        private void SearchTextModels_TextChanged(object sender, EventArgs e)
        {
            ModelsCollectionView.FilterString = "Contains([Name], '" + SearchTextModels.Text + "')";
        }
    }
}