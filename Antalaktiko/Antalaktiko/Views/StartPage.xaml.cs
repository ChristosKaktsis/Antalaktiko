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
            
           
        }

        private void OpenPopUp_Clicked(object sender, EventArgs e)
        {
            filterpopup.IsOpen = true;
            
            _viewModel.LoadPartItemsCommand.Execute(null);
        }

        private void ClosePopup_Clicked(object sender, EventArgs e)
        {
            _viewModel.FilterCollectionCommand.Execute(null);
            filterpopup.IsOpen = false;          
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            
            if(choiceChipGroup.SelectedChip!=null)
                choiceChipGroup.SelectedChip.IsSelected = false;
            _viewModel.SelectedBrand = null;
            YearFromComboBox.SelectedIndex = -1;
            YearToComboBox.SelectedIndex = -1;
            FuelTypeComboBox.SelectedIndex = -1;
            PartItemComboBox.SelectedIndex = -1;
        }

        private void Brand_Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var brand = _viewModel.SelectedBrand;
            if (brand == null)
                return;
            
        }
    }
}