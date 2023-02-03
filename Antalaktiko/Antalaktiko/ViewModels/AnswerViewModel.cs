using Antalaktiko.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
       
        private string author_Id;

        public ObservableCollection<Comment> CommentCollection { get; set; }
        public List<Comment> NewComments { get; set; }
        public ObservableCollection<Models.Image> ImageList { get; set; } = new ObservableCollection<Models.Image>();
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
        public string Author_Id
        {
            get => author_Id;
            set => SetProperty(ref author_Id, value);
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
                //Author_Id = item.Author;
                Description = item.Data.Desc;
                //Author_Name = item.author_name;
                TitleInfo = item.Data.Brand_name;
                Brand = item.Data.Brand_name;
                Model = item.Data.Model_name;
                Company = item.Data.Company_name;
                Chronology = item.Data.Date_from;
                Type = item.Type;
                Fuel = item.Data.Fuel;
                Doors = item.Data.Doors;
                PartType = item.Data.Cat_name;
                //ItemState = item.Data.State_string;
                await Task.Delay(1000);
                item.Data?.Images?.ForEach(i => ImageList.Add(i));
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        public async Task LoadComments()
        {
            try
            {
                CommentCollection.Clear();
                //new comments added after answer
                var list = NewComments.OrderByDescending(x => x.Date).ToList();
                list.ForEach(CommentCollection.Add);
                //load comments from service
                var comments = await commentManager.GetCommentWithPostId(itemId);
                
                //put to collection only first level parent comments 
                foreach (var item in comments)
                    if (item.Author.ToString() == LogedUser || Author_Id == LogedUser)
                        CommentCollection.Add(item);

                //put empty comment to UI
                if (!CommentCollection.Any())
                    CommentCollection.Add(new Comment());

                //add children to comments 
                foreach (var c in comments)
                    AddChild(c);

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void AddChild(Comment c)
        {
            try
            {
                if (c.Child != null)
                    return;
                var child = CommentCollection.Where(x => x.Parent == c.Id).FirstOrDefault();
                if (child == null)
                    return;
                c.Child = child;
                AddChild(child);
            }
            catch { Debug.WriteLine("AddChild wrong"); }
            
        }

        public async Task AnswerComment(string comId, string answer)
        {
            try
            {
                var comment = new 
                {
                    Author = LogedUser,
                    Date = DateTime.Now.ToString(),
                    Description = answer,
                    Pid = itemId,
                    parent = comId
                };
              //  await commentManager.PutComment(comment);
                await Task.Delay(1);
                //normaly we would return an comment after put to display in list
                //temporary object creation
                NewComments.Add(new Comment
                {
                    Author_string = App.LogedUser?.Data?.FName,
                    Date = comment.Date,
                    Description = comment.Description
                });
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
