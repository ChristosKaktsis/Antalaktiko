using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class AnswerViewModel : BaseViewModel
    {
        private string itemId;
        private string description;
        private string author_Name;
        private string titleInfo;
        private string type;
        private string itemState;
        private string fuel;
        private string doors;
        private string partType;
        private string chronology;
        private string brand;
        private string model;
        private string company;

        public AnswerViewModel()
        {
            
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                SetProperty(ref itemId, value);
                LoadItemId(value);
            }
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public string Author_Name
        {
            get => author_Name;
            set => SetProperty(ref author_Name, value);
        }
        public string TitleInfo
        {
            get => titleInfo;
            set => SetProperty(ref titleInfo, value);
        }
        public string Brand
        {
            get => brand;
            set => SetProperty(ref brand, value);
        }
        public string Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }
        public string Company
        {
            get => company;
            set => SetProperty(ref company, value);
        }
        public string Chronology
        {
            get => chronology;
            set => SetProperty(ref chronology, value);
        }
        public string Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }
        public string ItemState
        {
            get => itemState;
            set => SetProperty(ref itemState, value);
        }
        public string Fuel
        {
            get => fuel;
            set => SetProperty(ref fuel, value);
        }
        public string Doors
        {
            get => doors;
            set => SetProperty(ref doors, value);
        }
        public string PartType
        {
            get => partType;
            set => SetProperty(ref partType, value);
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await postManager.GetItemWithId(itemId);
                Description = item.Description;
                Author_Name = item.author_name;
                TitleInfo = item.Info.Brand_Name;
                Brand = item.Info.Brand_Name;
                Model = item.Info.Model_Name;
                Company = item.company_name;
                Chronology = item.Info.Chronology;
                Type = item.Name;
                Fuel = item.Info.Fuel;
                Doors = item.Info.Doors;
                PartType = item.Info.part_categories_name;
                ItemState = item.Info.Item_State_Name;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to Load Item");
            }
        }
    }
}
