using SQLite;
using System;


namespace Antalaktiko.Models
{
    public class TK
    {
        [PrimaryKey, AutoIncrement]
        public int OID { get; set; }
        public string Ονοματκ { get; set; }
        public string Πόλη { get; set; }
        public string Νομός { get; set; }
        public string Περιοχή { get; set; }
    }
}
