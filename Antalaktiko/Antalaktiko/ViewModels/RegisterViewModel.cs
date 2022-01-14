using Antalaktiko.Models;
using Antalaktiko.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private bool isPopUpOpen;
        public IDataManger<Brand> brandManager = new BrandManager();
        public Command GoBackCommand { get; }
        public Command OpenPopUpCommand { get; }
        public ObservableCollection<Brand> BrandItems { get; }
        public RegisterViewModel()
        {
            GoBackCommand = new Command(OnGoBackClicked);
            OpenPopUpCommand = new Command(OnOpenPopUp);
            BrandItems = new ObservableCollection<Brand>();
        }

        private async void OnOpenPopUp(object obj)
        {
            //open pop up to select brands
            IsPopUpOpen = !IsPopUpOpen;
            //load brands
            await ExecuteLoadItemsCommand();
        }

        public bool IsPopUpOpen
        {
            get => isPopUpOpen;
            set => SetProperty(ref isPopUpOpen, value);
        }
        
        async Task ExecuteLoadItemsCommand()
        {

            IsBusy = true;

            try
            {
                BrandItems.Clear();
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
    }
}
