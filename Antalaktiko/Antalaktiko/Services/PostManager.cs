using Antalaktiko.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Antalaktiko.Services
{
    public class PostManger : BaseManager, IDataManger<Post>
    {
        static readonly int num = 15;
        static int page = 0;
        static readonly string Url = $"{BaseAddress}?method=Posts&params[num]={num}&params[page]=";
        

        public async Task<IEnumerable<Post>> GetAll()
        {
            page = 0;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url+"0");
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<IEnumerable<Post>> GetMore()
        {
            page += num;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url+$"{page}");       
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<Post> GetItemWithId(string id)
        {
            page += num;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[id]={id}");
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result).FirstOrDefault();
        }
        public async Task<bool> FilterSearch()
        {
            var book = new
            {
                SelectedType = "Θέλω να αγοράσω",
                SelectedBrand = "audi",
                SelectedModel = "a3",
                SelectedPart = "Είδη Φανοποίιας",
                SelectedYearFrom ="2006",
                SelectedYearTo ="2022",
                SelectedFuelType ="Βενζίνη",
            };
            var json = JsonConvert.SerializeObject(book);
            var loginurl = $"{BaseAddress}?putData=Filters&data={json}";
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(loginurl);
            return true;
        }
        public async Task<bool> GetUserPost()
        {
            var book = new
            {
               LogedUserID = "6199"
            };
            var json = JsonConvert.SerializeObject(book);
            var loginurl = $"{BaseAddress}?putData=Post&data={json}";
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(loginurl);
            return true;
        }
    }
}
