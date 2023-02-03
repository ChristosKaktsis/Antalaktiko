using Antalaktiko.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Antalaktiko.Services
{
    public class UserManager : BaseManager, IDataManger<User>
    {
        static readonly string Url = $"{BaseAddress}?method=Users";

        public async Task<IEnumerable<User>> GetAll()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<User>>(result);
        }
        public async Task<User> GetItem(string id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Users&params[id]={id}");
            return JsonConvert.DeserializeObject<IEnumerable<User>>(result).FirstOrDefault();
        }
        public async Task<User> LogIn(object loginobj)
        {

            var loginurl = $"{BaseAddress}scripts/add_functions.php";
            HttpClient client =  GetClient();
            var response = await client.PostAsync(loginurl,
               new StringContent(
                   JsonConvert.SerializeObject(loginobj),
                   Encoding.UTF8, "application/json"));
            Debug.WriteLine(await response.Content.ReadAsStringAsync());
            return JsonConvert.DeserializeObject<User>
                (await response.Content.ReadAsStringAsync());
        }
        public async Task<User> Login(string username, string password) 
        {
            var client = GetRestClient();
            //client.Timeout = -1;
            var request = new RestRequest();
            request.AlwaysMultipartFormData = true;
            request.AddParameter("type", "login");
            request.AddParameter("email", username);
            request.AddParameter("password", password);
            var response = await client.PostAsync(request);
            return JsonConvert.DeserializeObject<User>
                (response.Content);
        }
        public async Task<bool> Register(UserInfo user,CompanyInfo company)
        {
            var client = GetRestClient();
            //client.Timeout = -1;
            var request = new RestRequest();
            request.AlwaysMultipartFormData = true;
            request.AddParameter("type", "register");
            request.AddParameter("email", user.Email);
            request.AddParameter("password", user.Password);
            request.AddParameter("retype", user.Password);//wut?
            request.AddParameter("fname", user.FName);
            request.AddParameter("lname", user.LName);
            user.Brands.ForEach(brand => { request.AddParameter("brands", brand); });
            user.Parts.ForEach(part => { request.AddParameter("parts", part); });
            user.Condition.ForEach(condition => { request.AddParameter("condition", condition); });
            request.AddParameter("mobile", user.Mobile);
            request.AddParameter("terms_of_use", user.Terms_of_use);
            request.AddParameter("privacy_policy", user.Privacy_policy);
            request.AddParameter("cname", company.Cname);
            request.AddParameter("cwebsite", company.Website);
            request.AddParameter("cmail", company.Company_Email);
            request.AddParameter("cvat", company.Vat);
            request.AddParameter("caddress", company.Address);
            request.AddParameter("cpc", company.Post_Code);
            request.AddParameter("cphone", company.Phone);
            request.AddParameter("cmobile", company.Mobile);
            request.AddParameter("cjob", company.Job_id);
            request.AddParameter("cregion", company.Region);
            request.AddParameter("country", company.Country);

            var response = await client.PostAsync(request);
            return true;
        }

    }
}
