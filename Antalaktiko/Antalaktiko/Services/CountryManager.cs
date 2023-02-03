using Antalaktiko.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Antalaktiko.Services
{
    public class CountryManager : BaseManager, IDataManger<Country>
    {
        static readonly string Url = $"{BaseAddress}?method=Regions";

        public async Task<IEnumerable<Country>> GetAll()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Country>>(result);
        }
    }
}
