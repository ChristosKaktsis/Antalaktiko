using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.Models
{
    public class CompanyInfo
    {
        public string WebSite { get; set; }
        public string Email { get; set; }
        public string Vat { get; set; }
        public string Parts_String { get; set; }
        public string Address1 { get; set; }
        public string Post_Code { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Subscription_Started { get; set; }
        public string Subscription_Ends { get; set; }
        public string Item_Condition { get; set; }
        public string Brands_String { get; set; }
        public string Company_Job { get; set; }
        public string Accepted { get; set; }
        public string Active { get; set; }
        public string Seller { get; set; }
        public string Location { get; set; }
        public string Company_Type 
        {
            get
            {
                string type = string.Empty;
                switch (Company_Job)
                {
                    case "1":
                        type = "ΑΝΤΑΛΛΑΚΤΙΚΑ ΑΥΤΟΚΙΝΗΤΩΝ";
                        break;
                    case "2":
                        type = "ΑΝΤΑΛΛΑΚΤΙΚΑ AFTERMARKET";
                        break;
                    case "3":
                        type = "ΑΝΤΑΛΛΑΚΤΙΚΑ ΙΜΙΤΑΣΙΟΝ";
                        break;
                    case "4":
                        type = "ΑΝΤΑΛΛΑΚΤΙΚΑ ΜΕΤΑΧΕΙΡΣΜΕΝΑ";
                        break;
                    case "5":
                        type = "ΑΝΤΙΠΡΟΣΩΠΕΙΑ";
                        break;
                    case "6":
                        type = "ΑΣΦΑΛΙΣΤΙΚΗ ΕΤΑΙΡΙΑ";
                        break;
                    case "7":
                        type = "ΒΑΦΕΙΟ";
                        break;
                    case "8":
                        type = "ΕΤΑΙΡΙΑ ΕΝΟΙΚΙΑΣΕΩΝ";
                        break;
                    case "9":
                        type = "ΗΛΕΚΤΡΟΛΟΓΕΙΟ-ΔΙΑΓΝΩΣΤΙΚΟ ΚΕΝΤΡΟ";
                        break;
                    case "10":
                        type = "ΚΑΘΕΤΗ ΜΟΝΑΔΑ";
                        break;
                    case "11":
                        type = "ΜΙΖΕΣ-ΔΥΝΑΜΟ";
                        break;
                    case "12":
                        type = "ΣΥΝΕΡΓΕΙΟ ΑΥΤΟΚΙΝΗΤΩΝ";
                        break;
                    case "13":
                        type = "ΦΑΝΟΠΟΙΕΙΟ";
                        break;
                    case "14":
                        type = "ΨΥΓΕΙΑ ΑΥΤΟΚΙΝΗΤΩΝ";
                        break;
                }
                return type;
            } 
        }
    }
}
