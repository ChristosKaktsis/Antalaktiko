using Antalaktiko.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Antalaktiko.Services
{
    public class PostManger : BaseManager
    {
        static readonly int num = 6;
        static int page = 0;
        static readonly string Url = $"{BaseAddress}?method=Posts&params[num]={num}&params[page]=";


        //public async Task<IEnumerable<Post>> GetAll()
        //{
        //    page = 0;
        //    HttpClient client = GetClient();
        //    string result = await client.GetStringAsync(Url+"0");
        //    result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
        //    return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        //}
        //public async Task<IEnumerable<Post>> GetMore()
        //{
        //    page += num;
        //    HttpClient client = GetClient();
        //    string result = await client.GetStringAsync(Url + $"{page}");
        //    result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
        //    return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        //}
        public async Task<IEnumerable<Post>> GetAll(string brand = null,string model = null, string search = null)
        {
            page = 0;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(getUrl(page,brand, model, search));
            result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<IEnumerable<Post>> GetMore(string brand = null, string model = null, string search = null)
        {
            page += num;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(getUrl(page, brand, model, search));
            result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        private string getUrl(int page,string brand, string model, string search)
        {
            string url = Url + $"{page}";
            if(!string.IsNullOrEmpty(brand)) url+= $"&params[brand]={brand}";
            if(!string.IsNullOrEmpty(model)) url+= $"&params[model]={model}";
            if (!string.IsNullOrEmpty(search)) url += $"&params[desc_search]={search}";
            return url;
        }
        
        public async Task<Post> GetItemWithId(string id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[id]={id}");
            result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result).FirstOrDefault();
        }
        public async Task<IEnumerable<Post>> GetItemsWithUid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Enumerable.Empty<Post>(); 
            page = 0;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[uid]={id}&params[type]=mine&params[upage]={page}&params[unum]={num}");
            result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<IEnumerable<Post>> GetMoreItemsWithUid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Enumerable.Empty<Post>();
            page += num;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[uid]={id}&params[type]=mine&params[upage]={page}&params[unum]={num}");
            result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<IEnumerable<Post>> GetMyAnsweredWithUid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Enumerable.Empty<Post>();
            page = 0;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[uid]={id}&params[type]=my_answers&params[upage]={page}&params[unum]={num}");
            result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<IEnumerable<Post>> GetMoreMyAnsweredWithUid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return Enumerable.Empty<Post>();
            page += num;
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Posts&params[uid]={id}&params[type]=my_answers&params[upage]={page}&params[unum]={num}");
            result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
        public async Task<bool> RegisterPost(Post post, List<string> images = null)
        {
            if (post == null) return false;
            if(post.Data == null) return false;
            var client = GetRestClient();
            var request = new RestRequest();
            request.AlwaysMultipartFormData = true;
            request.AddParameter("type", "post");
            request.AddParameter("title", post.Type);
            request.AddParameter("desc", post.Data.Desc);
            request.AddParameter("date", DateTime.Now.ToString("ΥΥΥΥ-MM-DD H:i:s"));
            request.AddParameter("user_id", post.Data.User_id);
            request.AddParameter("company_id", post.Data.Company_id);
            request.AddParameter("fuel", post.Data.Fuel);
            request.AddParameter("brand", post.Data.Brand);
            request.AddParameter("model", post.Data.Model);
            request.AddParameter("date_from", post.Data.Date_from);
            request.AddParameter("date_to", post.Data.Date_to);
            request.AddParameter("cat", post.Data.Cat);
            request.AddParameter("part_number", post.Data.Part_number);
            request.AddParameter("engine_code", post.Data.Engine_code);
            request.AddParameter("vin", post.Data.Vin);
            request.AddParameter("doors", post.Data.Doors);
            //request.AddParameter("qnt", post.Data.Qnt);
            //request.AddParameter("price", post.Data.Price);
            foreach(var state in post.Data.State)
                request.AddParameter("state", state+1);
            foreach(var image in images)
                request.AddFile($"image{images.IndexOf(image)+1}", image);
            var response = await client.PostAsync(request);
            return true;
        }
    }
}
