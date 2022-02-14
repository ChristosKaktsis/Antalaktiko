using Antalaktiko.Models;
using Antalaktiko.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    public class CompaniesViewModel : BaseViewModel
    {
        public ObservableCollection<Company> CompanyCollection { get; set; }
        public Command LoadCompaniesCommand { get; set; }
        public Command LoadMoreCompaniesCommand { get; set; }
        public Command GoToDetailPageCommand { get; set; }
        public CompaniesViewModel()
        {
            CompanyCollection = new ObservableCollection<Company>();
            LoadCompaniesCommand = new Command(async () => await ExecuteLoadCompaniesCommand());
            LoadMoreCompaniesCommand = new Command(async () => await ExecuteLoadMoreCompaniesCommand());
            GoToDetailPageCommand = new Command(ExecuteGoToDetailPageCommand);
        }

        private async void ExecuteGoToDetailPageCommand(object company)
        {
            var item = company as Company;
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(CompanyDetailPage)}?{nameof(CompanyDetailViewModel.ItemId)}={item.Id}");
        }

        private async Task ExecuteLoadMoreCompaniesCommand()
        {
            try
            {
                if (IsBusy)
                    return;
               
                var items = await companyManager.GetAll();
                foreach (var item in items)
                    CompanyCollection.Add(item);
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

        private async Task ExecuteLoadCompaniesCommand()
        {
            try
            {
                IsBusy = true;
                CompanyCollection.Clear();
                var items = await companyManager.GetAll();
                foreach (var item in items)
                    CompanyCollection.Add(item);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
