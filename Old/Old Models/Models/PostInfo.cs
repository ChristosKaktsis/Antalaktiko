using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class PostInfo
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Chronology { get; set; }
        public string ChronilogyTo { get; set; }
        public string Fuel { get; set; }
        public string Doors { get; set; }
        public string Part_Categories { get; set; }
        public string Part_Number { get; set; }
        public string Engine_Code { get; set; }
        public string Vehicle_Id_Number { get; set; }
        public string Item_State { get; set; }
        public string Brand_Name { get; set; }
        public string Model_Name { get; set; }
        public string part_categories_name { get; set; }
        public string Item_State_Name 
        { 
            get 
            {
                string state = string.Empty;
                switch (Item_State)
                {
                    case "1":
                        state = "Ιμιτασιόν";
                        break;
                    case "2":
                        state = "Γνήσιο";
                        break;
                    case "3":
                        state = "Μεταχειρισμένο";
                        break;
                    case "4":
                        state = "Ανακατασκευασμένο";
                        break;
                    default:
                        state = "Όλα";
                        break;
                }
                return state;
            } 
        }
    }
}
