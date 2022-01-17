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
    public class CompanyManager : BaseManager, IDataManger<Company>
    {
        static readonly string Url = $"{BaseAddress}?method=Companies";
        public async Task<IEnumerable<Company>> GetAll()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Company>>(result);
        }
        public async Task<Company> GetItem(string id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Companies&params[id]={id}");
            return JsonConvert.DeserializeObject<IEnumerable<Company>>(result).FirstOrDefault();
        }
    }
}
