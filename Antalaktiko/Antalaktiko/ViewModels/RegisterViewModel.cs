using Antalaktiko.Models;
using Antalaktiko.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private bool isPopUpOpen, isPartsPopUpOpen, isAllChecked, isAllPartsChecked, isAfmPopUpOpen, afmHasError;      
        private TK selectedTK;
        private string selectedRegion, afm, afmError, addressNo, address, companyName, tk;
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
        public ObservableCollection<Brand> BrandItems { get; }
        public ObservableCollection<Part> PartItems { get; }
        public ObservableCollection<TK> TKItems { get; }
        public List<int> PartType { get; }
        public RegisterViewModel()
        {
            GoBackCommand = new Command(OnGoBackClicked);
            OpenPopUpCommand = new Command(OnOpenPopUp);
            OpenPartsPopUpCommand = new Command(OnOpenPartsPopUp);
            ClearSelectedBrandCommand = new Command(()=>CheckAllBrands(false));
            ClearSelectedPartsCommand = new Command(() => CheckAllParts(false));
            LoadFieldWithAFMCommand = new Command(async () => await ExecuteLoadFieldWithAFM());
            RegisterCommand = new Command(ExecuteRegisterCommand);
            BrandItems = new ObservableCollection<Brand>();
            PartItems = new ObservableCollection<Part>();
            TKItems = new ObservableCollection<TK>();
            PartType = new List<int>();
        }

        private async void ExecuteRegisterCommand(object obj)
        {
            List<int> selectedbrands = new List<int>();
            List<int> selectedparts = new List<int>();
            foreach (var braitem in BrandItems)
                if (braitem.IsChecked)
                    selectedbrands.Add(braitem.Id);

            foreach (var partitem in PartItems)
                if (partitem.IsChecked)
                    selectedparts.Add(partitem.Id);

            var regitem = new 
            {
                Afm, CompanyName, Website, Email, Telephone, MobilePhone,Fax,
                Address,AddressNo,SelectedTK.Ονοματκ,SelectedRegion,
                selectedbrands,selectedparts,PartType,CompanyType,
                Name,Surname,Userphone,UserMail,Password
            };
            await userManger.Register(regitem);
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
        //company info
        public TK SelectedTK
        {
            get => selectedTK;
            set 
            { 
                SetProperty(ref selectedTK, value);
                if (value != null)
                    SelectedRegion = value.Περιοχή;
            } 
        }
        public string SelectedRegion
        {
            get => selectedRegion;
            set => SetProperty(ref selectedRegion, value);
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
                SetProperty(ref allChecked, value);
                
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
                SetProperty(ref imitationChecked, value);
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
        public string Website
        {
            get => website;
            set
            {
                SetProperty(ref website, value);

            }
        }
        public string Email
        {
            get => email;
            set
            {
                SetProperty(ref email, value);

            }
        }
        public string Telephone
        {
            get => telephone;
            set
            {
                SetProperty(ref telephone, value);

            }
        }
        public string Fax
        {
            get => fax;
            set
            {
                SetProperty(ref fax, value);

            }
        }
        public string MobilePhone
        {
            get => mobilePhone;
            set
            {
                SetProperty(ref mobilePhone, value);

            }
        }
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
        
        //user info
        public string Name
        {
            get => name;
            set
            {
                SetProperty(ref name, value);

            }
        }
        public string Surname
        {
            get => surname;
            set
            {
                SetProperty(ref surname, value);

            }
        }
        public string Userphone
        {
            get => userphone;
            set
            {
                SetProperty(ref userphone, value);

            }
        }
        public string UserMail
        {
            get => usermail;
            set
            {
                SetProperty(ref usermail, value);

            }
        }
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);

            }
        }
        public async void OnAppearing()
        {
            await Task.WhenAll
                (ExecuteLoadBrandsCommand(),
                ExecuteLoadPartsCommand(),
                ExecuteLoadTKCommand()
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
