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
        public PostManger postManager = new PostManger();
        public CompanyManager companyManager = new CompanyManager();
        public UserManager userManager = new UserManager();
        private Brand selectedBrand;
        private Model selectedModel;
        private Part selectedPart;
        private string selectedYearFrom;
        private string selectedYearTo;
        private string selectedFuelType;
        private string searchFilter;
        private int selectedChipIndex = -1;

        public Command NewAdCommand { get; }
        public ObservableCollection<Post> PostItems { get; set; }
        public List<Post> SourcePostItems { get; set; }
        public List<User> UserItems { get; set; }
        public ObservableCollection<Brand> BrandItems { get; }
        public ObservableCollection<Model> ModelItems { get; }
        public ObservableCollection<Part> PartItems { get; }
        public List<int> YearsFrom { get; }
        public Command LoadPostItemsCommand { get; }
        public Command LoadBrandItemsCommand { get; }
        public Command LoadPartItemsCommand { get; }
        public Command FilterCollectionCommand { get; }
        public Command LoadMoreCommand { get; }
        public StartViewModel()
        {
            NewAdCommand = new Command(OnNewAdPressed);
            SourcePostItems = new List<Post>();
            UserItems = new List<User>();
            PostItems = new ObservableCollection<Post>();
            BrandItems = new ObservableCollection<Brand>();
            ModelItems = new ObservableCollection<Model>();
            PartItems = new ObservableCollection<Part>();
            YearsFrom = SetYears();
            LoadPostItemsCommand = new Command(async () => await ExecuteLoadPostItemsCommand());
            LoadMoreCommand = new Command(async () => await ExecuteLoadMoreCommand());
            LoadBrandItemsCommand = new Command(async () => await ExecuteLoadBrandItemsCommand());
            LoadPartItemsCommand = new Command(async () => await ExecuteLoadPartsCommand());
            FilterCollectionCommand = new Command(ExecuteFilterCollectionCommand);
        }
        
        private async Task ExecuteLoadMoreCommand()
        {
            if (IsBusy)
                return;
            if (IsRefresing)
                return;
            try
            {
                IsRefresing = true;
           
                var items = await postManager.GetMore();
                foreach (var item in items)
                {
                    //set Author Info to display
                    item.AuthorDesc = await SetAuthorDesc(item.Author);
                    //set title Info to display
                    item.TitleInfo = await SetPostTitleInfo(item.Info);
                    PostItems.Add(item);
                    //there is a Post with null and we cant filter it with out info 
                    if (item.Info != null)
                        SourcePostItems.Add(item);
                }
                ExecuteFilterCollectionCommand();
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefresing = false;
            }
        }

        private async Task<string> SetAuthorDesc(string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
                return string.Empty;

            var user = UserItems.Where(x => x.Id == authorId).FirstOrDefault();
            var author = await companyManager.GetItem(user.Info.CompanyId);

            return author==null? string.Empty:author.Title;
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
                await Task.WhenAll(
                    ExecuteLoadBrandItemsCommand(), 
                    ExecuteLoadUserItemsCommand()
                    );
               
                var items = await postManager.GetAll();
                foreach (var item in items)
                {
                    //set Author Info to display
                    item.AuthorDesc = await SetAuthorDesc(item.Author);
                    //set title Info to display
                    item.TitleInfo = await SetPostTitleInfo(item.Info);
                    PostItems.Add(item);
                    //there is a Post with null and we cant filter it with out info 
                    if (item.Info != null)
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
        private async Task ExecuteLoadUserItemsCommand()
        {
            try
            {
                var items = await userManager.GetAll();
                foreach (var item in items)
                {
                    if (!UserItems.Contains(item))
                        UserItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private async Task<string> SetPostTitleInfo(PostInfo info)
        {
            if(info==null)
                return await Task.FromResult(string.Empty);
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
        public int SelectedChipIndex
        {
            get => selectedChipIndex;
            set
            {
                SetProperty(ref selectedChipIndex, value);
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
                ExecuteFilterCollectionCommand();
            }
        }
        public void ExecuteFilterCollectionCommand()
        {
            var BrandFilter = SelectedBrand != null ? SelectedBrand.Id : string.Empty;
            var ModelFilter = SelectedModel != null ? SelectedModel.Id : string.Empty;
            var YearFilter = !string.IsNullOrEmpty(SelectedYearFrom) ? int.Parse(SelectedYearFrom) : 0;
            var YearFilterTo = !string.IsNullOrEmpty(SelectedYearTo) ? int.Parse(SelectedYearTo) : 2080;
            var BuySell = SelectedChipIndex ==-1 ? string.Empty : (SelectedChipIndex == 0 ? "Α" : "Π");
            var FilterText = !string.IsNullOrWhiteSpace(SearchFilter) ? SearchFilter : string.Empty;
            var filtereditems = SourcePostItems.Where(x => x.Info.Brand.Contains(BrandFilter));
            filtereditems = filtereditems.Where(x => x.Info.Model.Contains(ModelFilter));
            filtereditems = filtereditems.Where(x => int.Parse(x.Info.Chronology) >= YearFilter);
            filtereditems = filtereditems.Where(x => int.Parse(x.Info.Chronology) <= YearFilterTo);
            filtereditems = filtereditems.Where(x => x.Type.ToLowerInvariant().Contains(BuySell.ToLowerInvariant()));
            filtereditems = filtereditems.Where(x => x.TitleInfo.ToLowerInvariant().Contains(FilterText.ToLowerInvariant()));

            PostItems.Clear();
            foreach (var post in filtereditems)
            {
                PostItems.Add(post);
            }

            //foreach (var post in SourcePostItems)
            //{
            //    if (!filtereditems.Contains(post))
            //        PostItems.Remove(post);
            //    else
            //    {
            //        if (!PostItems.Contains(post))
            //            PostItems.Add(post);
            //    }
            //}
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
