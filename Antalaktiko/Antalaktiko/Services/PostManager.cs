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
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[id]={id}");
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result).FirstOrDefault();
        }
        public async Task<IEnumerable<Post>> GetItemsWithUid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Enumerable.Empty<Post>(); 
            page = 0;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[uid]={id}&params[type]=mine&params[upage]={page}&params[unum]={num}");
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<IEnumerable<Post>> GetMoreItemsWithUid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Enumerable.Empty<Post>();
            page += num;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[uid]={id}&params[type]=mine&params[upage]={page}&params[unum]={num}");
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<IEnumerable<Post>> GetMyAnsweredWithUid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Enumerable.Empty<Post>();
            page = 0;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[uid]={id}&params[type]=my_answers&params[upage]={page}&params[unum]={num}");
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<IEnumerable<Post>> GetMoreMyAnsweredWithUid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Enumerable.Empty<Post>();
            page += num;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[uid]={id}&params[type]=my_answers&params[upage]={page}&params[unum]={num}");
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<IEnumerable<Post>> FilterSearch(object filter)
        {
            var json = JsonConvert.SerializeObject(filter);
            var loginurl = $"{BaseAddress}?putData=Filters&data={json}";
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(loginurl);
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<bool> RegisterPost(object post)
        {
            
            var json = JsonConvert.SerializeObject(post);
            var loginurl = $"{BaseAddress}?putData=Post&data={json}";
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(loginurl);
            return true;
        }
    }
}
