using System.Collections.Generic;

namespace Antalaktiko.Models
{
    public class Brand: BaseItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Model> Models { get; set; }       
    }
}
