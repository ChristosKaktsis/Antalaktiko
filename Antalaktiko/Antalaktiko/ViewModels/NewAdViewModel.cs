using Antalaktiko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    public class NewAdViewModel : BaseViewModel
    {
        private Brand selectedBrand;
        private Model selectedModel;
        private string selectedModelName = "Μοντέλο";
        private string selectedBrandName = "Μάρκα";

        public ObservableCollection<Brand> BrandItems { get; }
        public ObservableCollection<Model> ModelItems { get; }
        public ObservableCollection<Part> PartItems { get; }
        public List<int> YearsFrom { get; }
        public Command LoadBrandsCommand { get; }
        public NewAdViewModel()
        {
            BrandItems = new ObservableCollection<Brand>();
            ModelItems = new ObservableCollection<Model>();
            PartItems = new ObservableCollection<Part>();
            LoadBrandsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            YearsFrom = SetYears();
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
        public void OnAppearing()
        {
            LoadBrandsCommand.Execute(null);
            SetYears();
        }
        List<int> SetYears()
        {
            var minyearint = 1950;
            var maxyearint = int.Parse(DateTime.Now.ToString("yyyy"));
            var howmanyyears = maxyearint - minyearint +1;
            return  Enumerable.Range(minyearint, howmanyyears).ToList();
        }
    }
}
