using SQLite;
using System.Collections.Generic;

namespace Antalaktiko.Models
{
    public class Brand: BaseItem
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        [Ignore]
        public List<Model> Models { get; set; }       
    }
}
