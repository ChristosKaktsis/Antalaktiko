using Antalaktiko.Models;
using Antalaktiko.Services;
using Antalaktiko.Views;
using System;
using System.Collections.Generic;
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
        private Brand selectedBrand;
        private Model selectedModel;
        private Part selectedPart;
        private string selectedYearFrom;
        private string selectedYearTo;
        private string selectedFuelType;
        private string searchFilter;

        public Command NewAdCommand { get; }
        public ObservableCollection<Post> PostItems { get; set; }
        public List<Post> SourcePostItems { get; set; }
        public ObservableCollection<Brand> BrandItems { get; }
        public ObservableCollection<Model> ModelItems { get; }
        public ObservableCollection<Part> PartItems { get; }
        public List<int> YearsFrom { get; }
        public Command LoadPostItemsCommand { get; }
        public Command LoadBrandItemsCommand { get; }
        public Command LoadPartItemsCommand { get; }
        public Command FilterCollectionCommand { get; }
        public StartViewModel()
        {
            NewAdCommand = new Command(OnNewAdPressed);
            SourcePostItems = new List<Post>();
            PostItems = new ObservableCollection<Post>();
            BrandItems = new ObservableCollection<Brand>();
            ModelItems = new ObservableCollection<Model>();
            PartItems = new ObservableCollection<Part>();
            YearsFrom = SetYears();
            LoadPostItemsCommand = new Command(async () => await ExecuteLoadPostItemsCommand());
            LoadBrandItemsCommand = new Command(async () => await ExecuteLoadBrandItemsCommand());
            LoadPartItemsCommand = new Command(async () => await ExecuteLoadPartsCommand());
            FilterCollectionCommand = new Command(ExecuteFilterCollectionCommand);
        }

        

        private async Task ExecuteLoadBrandItemsCommand()
        {
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
                Debug.WriteLine(ex);
            }
        }
        private async Task ExecuteLoadPostItemsCommand()
        {
            IsBusy = true;
            try
            {
                SourcePostItems.Clear();
                PostItems.Clear();
                await ExecuteLoadBrandItemsCommand();
                var items = await postManager.GetAll();
                foreach (var item in items)
                {
                    var user = await userManager.GetItem(item.Author);
                    var author = await companyManager.GetItem(user.Info.CompanyId);
                    item.AuthorDesc = author.Title;
                    item.TitleInfo = await SetPostTitleInfo(item.Info);
                    PostItems.Add(item);
                    SourcePostItems.Add(item);
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
        private async Task<string> SetPostTitleInfo(PostInfo info)
        {
            string title = string.Empty;
            var brandname = BrandItems.Where(x => x.Id == info.Brand).FirstOrDefault();
            var modelname = brandname.Models.Where(x => x.Id == info.Model).FirstOrDefault();
            var chronology = info.Chronology;
            title = $"{brandname.Name} {modelname.Name} {chronology}";
            return await Task.FromResult(title);
        }
        private async Task ExecuteLoadPartsCommand()
        {          
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
        }
        public Brand SelectedBrand
        {
            get => selectedBrand;
            set
            {
                SetProperty(ref selectedBrand, value);
                SetBrandModels(value);
            }
        }
        public Model SelectedModel
        {
            get => selectedModel;
            set
            {
                SetProperty(ref selectedModel, value);
            }
        }
        public Part SelectedPart
        {
            get => selectedPart;
            set
            {
                SetProperty(ref selectedPart, value);
            }
        }
        public string SelectedYearFrom
        {
            get => selectedYearFrom;
            set
            {
                SetProperty(ref selectedYearFrom, value);
            }
        }
        public string SelectedYearTo
        {
            get => selectedYearTo;
            set
            {
                SetProperty(ref selectedYearTo, value);
            }
        }
        public string SelectedFuelType
        {
            get => selectedFuelType;
            set
            {
                SetProperty(ref selectedFuelType, value);
            }
        }
        private void SetBrandModels(Brand value)
        {
            ModelItems.Clear();
            SelectedModel = null;
            if (value == null)
                return;

            foreach (var item in value.Models)
                ModelItems.Add(item);
        }
        List<int> SetYears()
        {
            var minyearint = 1950;
            var maxyearint = int.Parse(DateTime.Now.ToString("yyyy"));
            var howmanyyears = maxyearint - minyearint + 1;
            return Enumerable.Range(minyearint, howmanyyears).OrderByDescending(i => i).ToList();
        }
        public string SearchFilter
        {
            get => searchFilter;
            set
            {
                SetProperty(ref searchFilter, value);
            }
        }
        public void ExecuteFilterCollectionCommand()
        {
            var BrandFilter = SelectedBrand != null ? SelectedBrand.Name : string.Empty;
            var filtereditems = SourcePostItems.Where(x => x.TitleInfo.Contains(BrandFilter));
            foreach(var post in SourcePostItems)
            {
                if (!filtereditems.Contains(post))
                    PostItems.Remove(post);
                else
                {
                    if (!PostItems.Contains(post))
                        PostItems.Add(post);
                }
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
