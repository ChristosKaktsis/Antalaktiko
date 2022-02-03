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
    public class UserManager : BaseManager, IDataManger<User>
    {
        static readonly string Url = $"{BaseAddress}?method=Users";

        public async Task<IEnumerable<User>> GetAll()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<User>>(result);
        }
        public async Task<User> GetItem(string id)
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}?method=Users&params[id]={id}");
            return JsonConvert.DeserializeObject<IEnumerable<User>>(result).FirstOrDefault();
        }
        public async Task<bool> LogIn()
        {
            var book = new { ΗλεκτρονικήΔιεύθυνση = "Christos@gmail.com", Συνθηματικό = "123123123" };
            var json = JsonConvert.SerializeObject(book);
            var loginurl = $"{BaseAddress}?putData=Login&data={json}";
            HttpClient client =  GetClient();
            string result = await client.GetStringAsync(loginurl);
            return true;
        }

    }
}
