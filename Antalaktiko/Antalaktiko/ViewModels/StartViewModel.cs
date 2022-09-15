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
        public UserManager userManager = new UserManager();
        private Brand selectedBrand;
        private Model selectedModel;
        private Part selectedPart;
        private string selectedYearFrom;
        private string selectedYearTo;
        private string selectedFuelType;
        private string searchFilter;
        private int selectedChipIndex = -1;
        private bool isFilterFocused;
        private int selectedFuelTypeIndex;

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
        public Command AnswerCommand { get; }
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
            AnswerCommand = new Command(ExecuteAnswerCommand);
            
        }

        private async void ExecuteAnswerCommand(object obj)
        {
            var item = obj as Post;
            await Shell.Current.GoToAsync($"{nameof(AnswerPage)}?{nameof(AnswerViewModel.ItemId)}={item.Id}");
        }

        private async Task ExecuteLoadBrandItemsCommand()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                if (BrandItems.Any())
                    return;
                //var items = await brandManager.GetAll();
                var items = await App.Database.GetBrandsAsync();
                var models = await App.Database.GetModelsAsync();
                foreach (var item in items)
                {
                    //add the brand s models
                    await AddBrandModels(models, item);
                    //add brand to collection
                    BrandItems.Add(item);
                }
                stopwatch.Stop();

                var elapsed = stopwatch.Elapsed;
                Debug.WriteLine("Load Brands Complete:" + elapsed);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private  Task AddBrandModels(List<Model> models, Brand item)
        {
            if (!models.Any())
                return null;
            item.Models = new List<Model>();
            
            foreach(var model in models)
            {
                //add matching model
                if (model.Brand == item.Id)
                    item.Models.Add(model);
            }
            return Task.FromResult(true);
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
        private async Task ExecuteLoadPostItemsCommand()
        {
            if (IsRefresing)
                return;
            var stopwatch = Stopwatch.StartNew();
            IsBusy = true;
            try
            {
                SourcePostItems.Clear();
                PostItems.Clear();
                await Task.WhenAll(
                    ExecuteLoadBrandItemsCommand());

                var items = await postManager.GetAll();
                foreach (var item in items)
                {
                    item.TitleInfo = await SetPostTitleInfo(item.Info);
                    PostItems.Add(item);
                    //there is a Post with null and we cant filter it with out info 
                    if (item.Info != null)
                        SourcePostItems.Add(item);
                }
                stopwatch.Stop();

                var elapsed = stopwatch.Elapsed;
                Debug.WriteLine("Load post Complete:"+elapsed);

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
                IsRefresing = false;
            }
        }
        private async Task<string> SetPostTitleInfo(PostInfo info)
        {
            if(info==null)
                return await Task.FromResult(string.Empty);
            string title = string.Empty;
            
            var chronology = info.Chronology;
            title = $"{info.Brand_Name} {info.Model_Name} {chronology}";
            return await Task.FromResult(title);
        }
        private async Task ExecuteLoadPartsCommand()
        {          
            try
            {
                if (PartItems.Any())
                    return;
                //var items = await partManager.GetAll();
                var items = await App.Database.GetPartsAsync();
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
        public int SelectedFuelTypeIndex
        {
            get => selectedFuelTypeIndex;
            set
            {
                SetProperty(ref selectedFuelTypeIndex, value);
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
        public bool IsFilterFocused
        {
            get => isFilterFocused;
            set
            {
                SetProperty(ref isFilterFocused, value);
            }
        }
        public async void ExecuteFilterCollectionCommand()
        {
            var buysell = SelectedChipIndex + 1;
            var brandid = SelectedBrand == null ? string.Empty : SelectedBrand.Id.ToString();
            var modelid = SelectedModel == null ? string.Empty : SelectedModel.Id.ToString();
            var partid = SelectedPart == null ? string.Empty : SelectedPart.Id.ToString();
            var yearfrom = string.IsNullOrEmpty(SelectedYearFrom) ? string.Empty : SelectedYearFrom;
            var yearto = string.IsNullOrEmpty(SelectedYearTo) ? string.Empty : SelectedYearTo;
            //var fuel = string.IsNullOrEmpty(selectedFuelType) ? string.Empty : SelectedFuelType;
            var fuel = SelectedFuelTypeIndex + 1;
            var filteritem = new
            {
                buysell,
                brandid,
                modelid,
                partid,
                yearfrom,
                yearto,
                fuel,
                page = "0",
                num = "15"
            };
            await ExecuteLoadFilteredPostItemsCommand(filteritem);
        }
        private async Task ExecuteLoadFilteredPostItemsCommand(object filteritem)
        {
            IsRefresing = true;
            IsBusy = true;
            try
            {
                SourcePostItems.Clear();
                PostItems.Clear();
                var items = await postManager.FilterSearch(filteritem);
                foreach (var item in items)
                {
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
                IsRefresing = IsBusy = false;
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
