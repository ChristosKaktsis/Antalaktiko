using Antalaktiko.Models;
using Antalaktiko.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Antalaktiko.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private bool isPopUpOpen, isPartsPopUpOpen, isAllChecked, isAllPartsChecked, isAfmPopUpOpen, afmHasError;      
        private TK selectedTK;
        private string afm, afmError, addressNo, address, companyName, tk;
        private string name, usermail, password, userphone, surname, mobilePhone, website, email, telephone, fax;
        private bool allChecked, imitationChecked, newChecked, usedChecked, rebuildChecked;
        private string compType;

        public Command GoBackCommand { get; }
        public Command OpenPopUpCommand { get; }
        public Command OpenPartsPopUpCommand { get; }
        public Command ClearSelectedBrandCommand { get; }
        public Command ClearSelectedPartsCommand { get; }
        public Command LoadFieldWithAFMCommand { get; }
        public Command RegisterCommand { get; }
        public ObservableCollection<Brand> BrandItems { get; } = new ObservableCollection<Brand>();
        public ObservableCollection<Part> PartItems { get; } = new ObservableCollection<Part>();
        public ObservableCollection<TK> TKItems { get; } = new ObservableCollection<TK>();
        public ObservableCollection<Country> CountryItems { get; } = new ObservableCollection<Country>();
        public ObservableCollection<Models.Region> RegionItems { get; } = new ObservableCollection<Models.Region>();
        public List<int> PartType { get; } = new List<int>();
        public RegisterViewModel()
        {
            GoBackCommand = new Command(OnGoBackClicked);
            OpenPopUpCommand = new Command(OnOpenPopUp);
            OpenPartsPopUpCommand = new Command(OnOpenPartsPopUp);
            ClearSelectedBrandCommand = new Command(()=>CheckAllBrands(false));
            ClearSelectedPartsCommand = new Command(() => CheckAllParts(false));
            LoadFieldWithAFMCommand = new Command(async () => await ExecuteLoadFieldWithAFM());
            RegisterCommand = new Command(Register);
        }
        private async void Register(object obj)
        {
            List<int> selectedbrands = new List<int>();
            List<int> selectedparts = new List<int>();
            foreach (var braitem in BrandItems)
                if (braitem.IsChecked)
                    selectedbrands.Add(braitem.Id);

            foreach (var partitem in PartItems)
                if (partitem.IsChecked)
                    selectedparts.Add(partitem.Id);
            try
            {
                var company = new CompanyInfo
                {
                    Cname = CompanyName,
                    Website = Website,
                    Company_Email = Email,
                    Vat = Afm,
                    Address = Address,
                    Post_Code = SelectedTK.Ονοματκ,
                    Region = SelectedRegion.Id.ToString(),
                    Phone = Telephone,
                    Mobile = MobilePhone,
                    Country = Selected_Country.Name,
                    Job_id = ""
                };
                var user = new UserInfo
                {
                    Email = UserMail,
                    Password = Password,
                    Mobile = Userphone,
                    FName = Name,
                    LName = Surname,
                    Brands = selectedbrands,
                    Parts = selectedparts,
                    Condition = PartType,
                    Terms_of_use = Terms_of_use ? "yes" : "no",
                    Privacy_policy = Privacy_Policy ? "yes" : "no"
                };
               var result = await userManger.Register(user, company);
            }
            catch(Exception ex) { Debug.WriteLine(ex); }
        }
        private async Task ExecuteLoadFieldWithAFM()
        {
            IsBusy = true;
            try
            {
                var afmService = new CallAfmService(Afm);
                CompanyName = await afmService.GetInfoFromXML("onomasia");
                Address = await afmService.GetInfoFromXML("postal_address");
                AddressNo = await afmService.GetInfoFromXML("postal_address_no");
                TK = await afmService.GetInfoFromXML("postal_zip_code");
                AfmError = await afmService.GetInfoFromXML("error_descr");
                IsAfmPopUpOpen = !string.IsNullOrEmpty(AfmError);
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
        private  void OnOpenPopUp(object obj)
        {
            //open pop up to select brands
            IsPopUpOpen = !IsPopUpOpen;
            //load brands
            
        }
        private  void OnOpenPartsPopUp(object obj)
        {
            //open pop up to select brands
            IsPartsPopUpOpen = !IsPartsPopUpOpen;
            //load brands

        }
        private async Task ExecuteLoadBrandsCommand()
        {

            IsBusy = true;

            try
            {
                if (BrandItems.Any())
                    return;
                //var items = await brandManager.GetAll();
                var items = await App.Database.GetBrandsAsync();
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
        private async Task ExecuteLoadPartsCommand()
        {
            IsBusy = true;

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
            finally
            {
                IsBusy = false;
            }
        }
        private async Task LoadCountries()
        {
            try
            {
                CountryManager manager = new CountryManager();
                var items = await manager.GetAll();
                items.ForEach(Item =>
                {
                    CountryItems.Add(Item);
                });
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
        //company info
        public TK SelectedTK
        {
            get => selectedTK;
            set 
            { 
                SetProperty(ref selectedTK, value);
            } 
        }
        private Models.Region selectedRegion;
        public Models.Region SelectedRegion
        {
            get => selectedRegion;
            set 
            {
                SetProperty(ref selectedRegion, value);
                if (value == null) return;
                SelectedRegionTitle = value.Title;
            } 
        }
        private string _SelectedRegionTitle = "Περιοχή";

        public string SelectedRegionTitle
        {
            get { return _SelectedRegionTitle; }
            set { SetProperty(ref _SelectedRegionTitle, value); }
        }

        public bool IsAfmPopUpOpen
        {
            get => isAfmPopUpOpen;
            set => SetProperty(ref isAfmPopUpOpen, value);
        }
        public bool IsPopUpOpen
        {
            get => isPopUpOpen;
            set => SetProperty(ref isPopUpOpen, value);
        }
        public bool IsPartsPopUpOpen
        {
            get => isPartsPopUpOpen;
            set => SetProperty(ref isPartsPopUpOpen, value);
        }
        public bool IsAllBrandsChecked
        {
            get => isAllChecked;
            set 
            {
                SetProperty(ref isAllChecked, value);
                CheckAllBrands(value);
            } 
        }
        public bool IsAllPartsChecked
        {
            get => isAllPartsChecked;
            set
            {
                SetProperty(ref isAllPartsChecked, value);
                CheckAllParts(value);
            }
        }
        public bool AllChecked
        {
            get => allChecked;
            set 
            {
                if (value)
                    PartType.Add(0);
                else
                    PartType.Remove(0);
            } 
        }
        
        public bool ImitationChecked
        {
            get => imitationChecked;
            set 
            {
                if (value)
                    PartType.Add(1);
                else
                    PartType.Remove(1);
            } 
        }
        public bool NewChecked
        {
            get => newChecked;
            set 
            {
                SetProperty(ref newChecked, value);
                if (value)
                    PartType.Add(2);
                else
                    PartType.Remove(2);
            } 
        }
        public bool UsedChecked
        {
            get => usedChecked;
            set 
            {
                SetProperty(ref usedChecked, value);
                if (value)
                    PartType.Add(3);
                else
                    PartType.Remove(3);
            } 
        }
        public bool RebuildChecked
        {
            get => rebuildChecked;
            set 
            {
                SetProperty(ref rebuildChecked, value);
                if (value)
                    PartType.Add(4);
                else
                    PartType.Remove(4);
            } 
        }
        public string Afm
        {
            get => afm;
            set
            {
                SetProperty(ref afm, value);
                
            }
        }
        public string CompanyName
        {
            get => companyName;
            set
            {
                SetProperty(ref companyName, value);

            }
        }
        public string Address
        {
            get => address;
            set
            {
                SetProperty(ref address, value);

            }
        }
        public string AddressNo
        {
            get => addressNo;
            set
            {
                SetProperty(ref addressNo, value);

            }
        }
        public string AfmError
        {
            get => afmError;
            set
            {
                SetProperty(ref afmError, value);
                AfmHasError = !string.IsNullOrWhiteSpace(value);
            }
        }
        public bool AfmHasError
        {
            get => afmHasError;
            set => SetProperty(ref afmHasError, value);
        }
        public string TK
        {
            get => tk;
            set
            {
                SetProperty(ref tk, value);
                SetTK(value);
            }
        }
        private void SetTK(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            SelectedTK = TKItems.Where(x => x.Ονοματκ == value).FirstOrDefault();
        }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Telephone{get; set; }
        public string Fax{get;set; }
        public string MobilePhone { get; set; }
        public string CompanyType
        {
            get => compType;
            set
            {
                SetProperty(ref compType, value);

            }
        }
        private void CheckAllParts(bool value)
        {
            foreach (var item in PartItems)
                item.IsChecked = value;
        }

        private void CheckAllBrands(bool value)
        {
            foreach (var item in BrandItems)
                item.IsChecked = value;
        }
        private Country country;
        public Country Selected_Country 
        { 
            get=> country;
            set
            {
                country = value;
                LoadRegions(value);
            } 
        }
        private void LoadRegions(Country value)
        {
            if (value == null) return;
            try
            {
                RegionItems.Clear();
                value.Regions.ForEach(region =>
                {
                    RegionItems.Add(region);
                });
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }
        //user info
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Userphone { get; set; }
        public string UserMail { get; set; }
        public string Password { get; set; }
        public bool Terms_of_use { get; set; }
        public bool Privacy_Policy { get; set; }
        public async void OnAppearing()
        {
            await Task.WhenAll
                (ExecuteLoadBrandsCommand(),
                ExecuteLoadPartsCommand(),
                ExecuteLoadTKCommand(),
                LoadCountries()
                );
            await Task.Delay(200);
            IsAfmPopUpOpen = true;
        }

        private async Task ExecuteLoadTKCommand()
        {
            IsBusy = true;
            try
            {
                if (TKItems.Any())
                    return;
                var items = await App.Database.GetNotesAsync();
                foreach (var item in items)
                {
                    TKItems.Add(item);
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
