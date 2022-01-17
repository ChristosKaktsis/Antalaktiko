using Antalaktiko.Models;
using Antalaktiko.Services;
using Antalaktiko.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    class StartViewModel : BaseViewModel
    {
        public IDataManger<Post> postManager = new PostManger();
        public CompanyManager companyManager = new CompanyManager();
        public UserManager userManager = new UserManager();
        public Command NewAdCommand { get; }
        public ObservableCollection<Post> PostItems { get; }
        public Command LoadPostItemsCommand { get; }
        public StartViewModel()
        {
            NewAdCommand = new Command(OnNewAdPressed);
            PostItems = new ObservableCollection<Post>();
            LoadPostItemsCommand = new Command(async () => await ExecuteLoadPostItemsCommand());
        }
        async Task ExecuteLoadPostItemsCommand()
        {
            IsBusy = true;

            try
            {
                PostItems.Clear();
                var items = await postManager.GetAll();
                foreach (var item in items)
                {
                    var user = await userManager.GetItem(item.Author);
                    var author = await companyManager.GetItem(user.Info.CompanyId);
                    item.AuthorDesc = author.Title;
                    PostItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public  void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnNewAdPressed(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewAdPage));
        }
    }
}
