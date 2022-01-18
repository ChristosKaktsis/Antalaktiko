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
    public partial class StartPage : ContentPage
    {
        private StartViewModel _viewModel;

        public StartPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new StartViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void choiceChipGroup_SelectionChanged(object sender, EventArgs e)
        {
            string st = "Α";
            if (choiceChipGroup.SelectedIndex == 1)
                st = "Π";
            PostCollectionView.FilterString = "Contains([Type], '"+st+"')";
           
        }

        private void OpenPopUp_Clicked(object sender, EventArgs e)
        {
            filterpopup.IsOpen = true;
        }

        private void ClosePopup_Clicked(object sender, EventArgs e)
        {
            filterpopup.IsOpen = false;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PostCollectionView.FilterString = string.Empty;
            choiceChipGroup.SelectedChip.IsSelected = false;
        }
    }
}