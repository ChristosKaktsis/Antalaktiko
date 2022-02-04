using Antalaktiko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    public class NewAdViewModel : BaseViewModel
    {
        private Brand selectedBrand;
        private Model selectedModel;
        private string selectedModelName = "Μοντέλο";
        private string selectedBrandName = "Μάρκα";
        private ImageSource imageSource;
        private int selectedbuysell;
        private int selectedDoor;
        private string selectedfuel;
        private Part selectedPart;
        private int selectedPartType;
        private string partCode;
        private string bikeCode;
        private string spaceNumber;
        private string description;
        private int selectedYearFrom;
        private int selectedYearTo;

        public ObservableCollection<Brand> BrandItems { get; }
        public ObservableCollection<Model> ModelItems { get; }
        public ObservableCollection<Part> PartItems { get; }
        public List<int> YearsFrom { get; }
        public List<string> ImageSources { get; }
        public Command LoadBrandsCommand { get; }
        public Command LoadPartsCommand { get; }
        public Command TakePhotoCommand { get; }
        public Command RegisterAdCommand { get; }
        public NewAdViewModel()
        {
            BrandItems = new ObservableCollection<Brand>();
            ModelItems = new ObservableCollection<Model>();
            PartItems = new ObservableCollection<Part>();
            ImageSources = new List<string>();
            LoadBrandsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadPartsCommand = new Command(async () => await ExecuteLoadPartItemsCommand());
            TakePhotoCommand = new Command(async () => await TakePhotoAsync());
            RegisterAdCommand = new Command(ExecuteRegisterCommand);
            YearsFrom = SetYears();
        }

        private async void ExecuteRegisterCommand()
        {
            try
            {
                var door = SelectedDoor + 2;
                var brand = SelectedBrand == null ? string.Empty : SelectedBrand.Id.ToString();
                var model = SelectedModel == null ? string.Empty : SelectedModel.Id.ToString();
                var part = selectedPart == null ? string.Empty : SelectedPart.Id.ToString();

                var post = new
                {
                    BuySell,
                    brand,
                    model,
                    SelectedYearFrom,
                    SelectedYearTo,
                    door,
                    SelectedFuel,
                    part,
                    PartType,
                    PartCode,
                    BikeCode,
                    SpaceNumber,
                    Description,
                    ImageSources
                };
                await postManager.RegisterPost(post);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
           
        }

        private async Task ExecuteLoadPartItemsCommand()
        {
            IsBusy = true;
            try
            {
                PartItems.Clear();
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
            finally
            {
                IsBusy = false;
            }
        }
        async Task ExecuteLoadItemsCommand()
        {

            IsBusy = true;

            try
            {
                BrandItems.Clear();
                //var items = await brandManager.GetAll();
                var items = await App.Database.GetBrandsAsync();
                var models = await App.Database.GetModelsAsync();
                foreach (var item in items)
                {
                    //add the brand s models
                    await AddBrandModels(models, item);
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
        private Task AddBrandModels(List<Model> models, Brand item)
        {
            if (!models.Any())
                return null;
            item.Models = new List<Model>();

            foreach (var model in models)
            {
                //add matching model
                if (model.Brand == item.Id)
                    item.Models.Add(model);
            }
            return Task.FromResult(true);
        }
        public Brand SelectedBrand
        {
            get => selectedBrand;
            set
            {
                SetProperty(ref selectedBrand, value);
                SetBrandModels(value);
                if (value != null)
                    SelectedBrandName = value.Name;
                else
                    SelectedBrandName = "Μάρκα";
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
        public Model SelectedModel
        {
            get => selectedModel;
            set
            {
                SetProperty(ref selectedModel, value);
                if (value != null)
                    SelectedModelName = value.Name;
                else
                    SelectedModelName = "Μοντέλο";
            }
        }
        public string SelectedBrandName
        {
            get => selectedBrandName;
            set
            {
                SetProperty(ref selectedBrandName, value);
            }
        }
        public string SelectedModelName
        {
            get => selectedModelName;
            set
            {
                SetProperty(ref selectedModelName, value);
            }
        }
        public int SelectedBuySell
        {
            get => selectedbuysell;
            set
            {
                SetProperty(ref selectedbuysell, value);
            }
        }
        public string BuySell
        {
            get
            {
                string buysell = string.Empty;
                if (SelectedBuySell == 1)
                    buysell = "θέλω να αγοράσω";
                else
                {
                    buysell = "Θέλω να πουλήσω";
                }
                return buysell;
            }
        }
        public int SelectedYearFrom

        {
            get => selectedYearFrom;
            set
            {
                SetProperty(ref selectedYearFrom, value);
            }
        }
        public int SelectedYearTo

        {
            get => selectedYearTo;
            set
            {
                SetProperty(ref selectedYearTo, value);
            }
        }
        public int SelectedDoor

        {
            get => selectedDoor;
            set
            {
                SetProperty(ref selectedDoor, value);
            }
        }
        public string SelectedFuel
        {
            get => selectedfuel;
            set
            {
                SetProperty(ref selectedfuel, value);
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
        public int SelectedPartType
        {
            get => selectedPartType;
            set
            {
                SetProperty(ref selectedPartType, value);
            }
        }
        public string PartType
        {
            get
            {
                string part = string.Empty;
                switch (SelectedPartType)
                {
                    case 0:
                        part = "Καινούριο";
                        break;
                    case 1:
                        part = "Μεταχειρισμένο";
                        break;
                    case 2:
                        part = "Ιμιτασιόν";
                        break;
                    case 3:
                        part = "Ανακατασκευασμένο";
                        break;

                }
                return part;
            }
        }
        public string PartCode
        {
            get => partCode;
            set
            {
                SetProperty(ref partCode, value);
            }
        }
        public string BikeCode
        {
            get => bikeCode;
            set
            {
                SetProperty(ref bikeCode, value);
            }
        }
        public string SpaceNumber
        {
            get => spaceNumber;
            set
            {
                SetProperty(ref spaceNumber, value);
            }
        }
        public string Description
        {
            get => description;
            set
            {
                SetProperty(ref description, value);
            }
        }
        public void OnAppearing()
        {
            LoadBrandsCommand.Execute(null);
            LoadPartsCommand.Execute(null);
            SetYears();
        }
        List<int> SetYears()
        {
            var minyearint = 1950;
            var maxyearint = int.Parse(DateTime.Now.ToString("yyyy"));
            var howmanyyears = maxyearint - minyearint +1;
            return Enumerable.Range(minyearint, howmanyyears).OrderByDescending(i => i).ToList();
        }
        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                SetProperty(ref imageSource, value);
            }
        }

        public string PhotoPath { get; private set; }

        public async Task TakePhotoAsync()
        {
            try
            {
                imageSource = null;
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                Console.WriteLine($"CapturePhotoAsync THREW: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Console.WriteLine($"CapturePhotoAsync THREW: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
            ImageSource = ImageSource.FromFile(newFile);
            ConvertImage(PhotoPath);
        }
        void ConvertImage(string p)
        {
            if (string.IsNullOrEmpty(p))
                return;
            byte[] imageByte = File.ReadAllBytes(p);
            ImageSources.Add(Convert.ToBase64String(imageByte));
        }
    }
}
