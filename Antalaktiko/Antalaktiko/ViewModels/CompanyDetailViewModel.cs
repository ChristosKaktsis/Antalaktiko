using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class CompanyDetailViewModel : BaseViewModel
    {
        private string comp_Title, itemId, website, email, address, phone, mobile, job;

        public CompanyDetailViewModel()
        {
            
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadCompany(value);
            }
        }
        public string Comp_Title
        {
            get =>comp_Title;
            set => SetProperty(ref comp_Title, value);
        }
        public string Website
        {
            get => website;
            set => SetProperty(ref website, value);
        }
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }
        public string Mobile
        {
            get => mobile;
            set => SetProperty(ref mobile, value);
        }
        public string Job
        {
            get => job;
            set => SetProperty(ref job, value);
        }
        private async void LoadCompany(string id)
        {
            var item = await companyManager.GetItem(id);
            Comp_Title = item.Title;
            if (item.Info == null)
                return;
            Website = item.Info.WebSite;
            Email = item.Info.Email;
            Address = item.Info.Address1;
            Phone = item.Info.Phone;
            Mobile = item.Info.Mobile;
            Job = item.Info.Company_Type;
        }

        public void OnAppearing()
        {

        }
    }
}
