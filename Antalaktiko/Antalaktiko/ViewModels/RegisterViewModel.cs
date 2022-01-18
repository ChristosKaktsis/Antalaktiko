using Antalaktiko.Models;
using Antalaktiko.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private bool isPopUpOpen;      
        private bool isAllChecked;
        private TK selectedTK;
        private string selectedRegion;
        private bool isPartsPopUpOpen;
        private bool isAllPartsChecked;

        public Command GoBackCommand { get; }
        public Command OpenPopUpCommand { get; }
        public Command OpenPartsPopUpCommand { get; }
        public Command ClearSelectedBrandCommand { get; }
        public Command ClearSelectedPartsCommand { get; }
        public ObservableCollection<Brand> BrandItems { get; }
        public ObservableCollection<Part> PartItems { get; }
        public ObservableCollection<TK> TKItems { get; }
        public RegisterViewModel()
        {
            GoBackCommand = new Command(OnGoBackClicked);
            OpenPopUpCommand = new Command(OnOpenPopUp);
            OpenPartsPopUpCommand = new Command(OnOpenPartsPopUp);
            ClearSelectedBrandCommand = new Command(()=>CheckAllBrands(false));
            ClearSelectedPartsCommand = new Command(() => CheckAllParts(false));
            BrandItems = new ObservableCollection<Brand>();
            PartItems = new ObservableCollection<Part>();
            TKItems = new ObservableCollection<TK>();
        }
        private async void OnOpenPopUp(object obj)
        {
            //open pop up to select brands
            IsPopUpOpen = !IsPopUpOpen;
            //load brands
            await ExecuteLoadItemsCommand();
        }
        async Task ExecuteLoadItemsCommand()
        {

            IsBusy = true;

            try
            {
                if (BrandItems.Any())
                    return;
                var items = await brandManager.GetAll();
                foreach (var item in items)
                {
                    BrandItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void OnOpenPartsPopUp(object obj)
        {
            //open pop up to select brands
            IsPartsPopUpOpen = !IsPartsPopUpOpen;
            //load brands
            await ExecuteLoadPartsCommand();
        }
        private async Task ExecuteLoadPartsCommand()
        {
            IsBusy = true;

            try
            {
                if (PartItems.Any())
                    return;
                var items = await partManager.GetAll();
                foreach (var item in items)
                {
                    PartItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public TK SelectedTK
        {
            get => selectedTK;
            set 
            { 
                SetProperty(ref selectedTK, value);
                if (value != null)
                    SelectedRegion = value.Περιοχή;
            } 
        }
        public string SelectedRegion
        {
            get => selectedRegion;
            set => SetProperty(ref selectedRegion, value);
        }    
        public bool IsPopUpOpen
        {
            get => isPopUpOpen;
            set => SetProperty(ref isPopUpOpen, value);
        }
        public bool IsPartsPopUpOpen
        {
            get => isPartsPopUpOpen;
            set => SetProperty(ref isPartsPopUpOpen, value);
        }
        public bool IsAllBrandsChecked
        {
            get => isAllChecked;
            set 
            {
                SetProperty(ref isAllChecked, value);
                CheckAllBrands(value);
            } 
        }
        public bool IsAllPartsChecked
        {
            get => isAllPartsChecked;
            set
            {
                SetProperty(ref isAllPartsChecked, value);
                CheckAllParts(value);
            }
        }

        private void CheckAllParts(bool value)
        {
            foreach (var item in PartItems)
                item.IsChecked = value;
        }

        private void CheckAllBrands(bool value)
        {
            foreach (var item in BrandItems)
                item.IsChecked = value;
        }
        public async void OnAppearing()
        {
            IsBusy = true;
            try
            {
                if (TKItems.Any())
                    return;
                var items = await App.Database.GetNotesAsync();
                foreach (var item in items)
                {
                    TKItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
