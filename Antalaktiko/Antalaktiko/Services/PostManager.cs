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
        static readonly string Url = $"{BaseAddress}?method=Posts&params[from]=4_d";

        public async Task<IEnumerable<Post>> GetAll()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Post>>(result);
        }
    }
}
