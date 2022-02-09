using Antalaktiko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class AnswerViewModel : BaseViewModel
    {
        private string itemId, description, author_Name, titleInfo, type, itemState, 
            fuel, doors, partType, chronology, brand, model, company;
        private string commntAnswer;

        public ObservableCollection<Comment> CommentCollection { get; set; }
        public List<Comment> NewComments { get; set; }
        public AnswerViewModel()
        {
            CommentCollection = new ObservableCollection<Comment>();
            NewComments = new List<Comment>();
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
        public async Task LoadComments()
        {
            try
            {
                CommentCollection.Clear();
                var list = NewComments.OrderByDescending(x => x.Date).ToList();
                list.ForEach(CommentCollection.Add);
                var comments = await commentManager.GetCommentWithPostId(itemId);
                foreach (var item in comments)
                    CommentCollection.Add(item);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public string CommentAnswer
        {
            get => commntAnswer;
            set => SetProperty(ref commntAnswer, value);
        }
        public async Task AnswerComment()
        {
            try
            {
                var comment = new 
                {
                    Author = LogedUser,
                    Date = DateTime.Now.ToString(),
                    Description = CommentAnswer,
                    Pid = itemId
                };
                //await commentManager.PutComment(comment);
                await Task.Delay(1);
                //normaly we would return an comment after put to display in list
                //temporary object creation
                NewComments.Add(new Comment
                {
                    Author_Name = App.LogedUser.First_Name,
                    Date = comment.Date,
                    Description = comment.Description
                });
                CommentAnswer = string.Empty;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
