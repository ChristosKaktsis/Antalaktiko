using Antalaktiko.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Antalaktiko.Services
{
    public class CommentManager : BaseManager
    {
        static readonly string Url = $"{BaseAddress}?method=Comments";
        public async Task<IEnumerable<Comment>> GetAll()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(result);
        }
        public async Task<IEnumerable<Comment>> GetCommentWithPostId(string postid)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url + $"&params[pid]={postid}");
            result = System.Web.HttpUtility.HtmlDecode(result);//this is for the char that appears &amp
            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(result);
        }
        public async Task<bool> PostComment(Comment item)
        {
            if (item == null) return false;
            var client = GetRestClient();
            var request = new RestRequest();
            request.AlwaysMultipartFormData = true;
            request.AddParameter("type", "comment");
            var response = await client.PostAsync(request);
            return true;
        }
    }
}
