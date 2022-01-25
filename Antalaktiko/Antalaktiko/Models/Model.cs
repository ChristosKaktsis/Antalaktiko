
using SQLite;

namespace Antalaktiko.Models
{
    public class Model
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Brand { get; set; }
    }
}
