using Antalaktiko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
                var brand = SelectedBrand == null ? string.Empty : SelectedBrand?.Id.ToString();
                var model = SelectedModel == null ? string.Empty : SelectedModel?.Id.ToString();
                var part = SelectedPart == null ? string.Empty : SelectedPart?.Id.ToString();
                var cat = SelectedPartType;
                var post = new Post
                {
                    Type = BuySell,
                    Data = new PostInfo
                    {
                        Desc = Description,
                        User_id = App.LogedUser.Id,
                        Fname = App.LogedUser?.Data?.FName,
                        Company_id = App.LogedUser?.Data?.Cid,
                        Fuel = SelectedFuel,
                        Brand = brand,
                        Brand_name = SelectedBrand?.Name,
                        Model = model,
                        Model_name = SelectedModel?.Name,
                        Date_from = SelectedYearFrom.ToString(),
                        Date_to = SelectedYearTo.ToString(),
                        Cat = part,
                        Cat_name = SelectedPart?.Title.ToString(),
                        Doors = (SelectedDoor + 2).ToString(),
                        Vin = SpaceNumber,
                        Engine_code = EngineCode,
                        Part_number = PartCode,
                        Price = "2",
                        Qnt= "2",
                        State = SelectedPartType.ToList()
                    }
                };
                await postManager.RegisterPost(post,ImageSources);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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
                Debug.WriteLine(ex);
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
                Debug.WriteLine(ex);
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
        public int SelectedBuySell { get; set; }
        
        public string BuySell
        {
            get
            {
                return SelectedBuySell == 1? "Θέλω να πουλήσω": "Θέλω να αγοράσω";
            }
        }
        public int SelectedYearFrom { get; set; }
        public int SelectedYearTo { get; set; }
        public int SelectedDoor { get; set; }
        public string SelectedFuel { get; set; }
        public Part SelectedPart { get; set; }
        public IList<int> SelectedPartType { get; set; }
        public string PartCode { get; set; } 
        public string EngineCode { get; set; }  
        public string SpaceNumber { get; set; } 
        public string Description { get; set; }
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

        public async Task PickPhotoAsync()
        {
            try
            {
                imageSource = null;
                var photo = await MediaPicker.PickPhotoAsync();
                await LoadPhotoAsync(photo);
                Debug.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                Debug.WriteLine($"CapturePhotoAsync THREW: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Debug.WriteLine($"CapturePhotoAsync THREW: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        public async Task TakePhotoAsync()
        {
            try
            {
                imageSource = null;
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Debug.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                Debug.WriteLine($"CapturePhotoAsync THREW: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Debug.WriteLine($"CapturePhotoAsync THREW: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
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
        void ConvertImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;
            byte[] imageByte = File.ReadAllBytes(path);
            ImageSources.Add(path);
        }
    }
}
