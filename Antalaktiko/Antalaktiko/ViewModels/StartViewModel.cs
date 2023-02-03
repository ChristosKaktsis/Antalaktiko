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
        private async Task ExecuteLoadPostItemsCommand()
        {
            if (IsRefresing)
                return;
            var stopwatch = Stopwatch.StartNew();
            IsBusy = true;
            try
            {
                PostItems.Clear();
                var items = await postManager.GetAll(
                    brand: SelectedBrand?.Id.ToString(),
                    model: SelectedModel?.Id.ToString(),
                    search:SearchFilter);
                foreach (var item in items)
                    PostItems.Add(item);
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

                var items = await postManager.GetMore(
                    brand: SelectedBrand?.Id.ToString(),
                    model: SelectedModel?.Id.ToString(),
                    search: SearchFilter);
                foreach (var item in items)
                    PostItems.Add(item);
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
        public string SearchFilter{get; set;}
        public bool IsFilterFocused{get;set;}
        public void ExecuteFilterCollectionCommand()
        {
            //var buysell = SelectedChipIndex + 1;
            //var partid = SelectedPart == null ? string.Empty : SelectedPart.Id.ToString();
            //var yearfrom = string.IsNullOrEmpty(SelectedYearFrom) ? string.Empty : SelectedYearFrom;
            //var yearto = string.IsNullOrEmpty(SelectedYearTo) ? string.Empty : SelectedYearTo;
            //var fuel = SelectedFuelTypeIndex + 1;
            IsBusy = true;
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        private async void OnNewAdPressed(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewAdPage));
        }
    }
}
