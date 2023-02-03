using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class CompanyDetailViewModel : BaseViewModel
    {
        private string comp_Title, itemId, website, email, address, phone, mobile, job;
        public Command PlacePhoneCallCommand { get; set; }
        public Command SendEmailCommand { get; set; }
        public CompanyDetailViewModel()
        {
            PlacePhoneCallCommand = new Command(()=>PlacePhoneCall(Phone));
            SendEmailCommand = new Command(async () => await SendEmail(new List<string> { Comp_Email}));
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
        public string Comp_Email
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
            Website = item.Info.Website;
            Comp_Email = item.Info.Company_Email;
            Address = item.Info.Address;
            Phone = item.Info.Phone;
            Mobile = item.Info.Mobile;
            Job = item.Info.Job_name;
        }
        public void PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
                Console.WriteLine(anEx);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Console.WriteLine(ex);
            }
        }
        public async Task SendEmail(List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    //Subject = subject,
                    //Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
                Console.WriteLine(fbsEx);
            }
            catch (Exception ex)
            {
                // Some other exception occurred
                Console.WriteLine(ex);
            }
        }
        public void OnAppearing()
        {

        }
    }
}
