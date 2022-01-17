using Antalaktiko.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Antalaktiko.Services
{
    public class PartManager : BaseManager, IDataManger<Part>
    {
        static readonly string Url = $"{BaseAddress}?method=Parts";

        public async Task<IEnumerable<Part>> GetAll()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Part>>(result);
        }
    }
}
