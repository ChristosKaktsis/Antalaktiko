using Antalaktiko.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    }
}
