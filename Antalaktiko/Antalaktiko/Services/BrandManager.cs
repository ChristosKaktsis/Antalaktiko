using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Antalaktiko.Models;

namespace Antalaktiko.Services
{
    public class BrandManager : BaseManager, IDataManger<Brand>
    {
        static readonly string Url = $"{BaseAddress}?method=Brands";
        public async Task<IEnumerable<Brand>> GetAll()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Brand>>(result);
        }
    }
}
