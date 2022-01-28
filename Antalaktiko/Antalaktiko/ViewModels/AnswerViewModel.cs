using System;
using System.Collections.Generic;
using System.Text;

namespace Antalaktiko.ViewModels
{
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

        public AnswerViewModel()
        {
            ItemId = "109600";
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
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
                Type = item.Type;
                fuel = item.Info.Fuel;
                Doors = item.Info.Doors;
                PartType = item.Info.part_categories_name;
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to Load Item");
            }
        }
    }
}
