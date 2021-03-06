using Antalaktiko.Models;
using Antalaktiko.Services;
using Antalaktiko.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    public class MyPostsViewModel : BaseViewModel
    {
        public ObservableCollection<Post> PostItems { get; set; }
        public List<Post> SourcePostItems { get; set; }
        public Command LoadPostItemsCommand { get; }
        public Command LoadMoreCommand { get; }
        public Command AnswerCommand { get; }
        
        public MyPostsViewModel()
        {
            PostItems = new ObservableCollection<Post>();
            SourcePostItems = new List<Post>();
            LoadPostItemsCommand = new Command(async () => await ExecuteLoadPostItemsCommand());
            LoadMoreCommand = new Command(async () => await ExecuteLoadMoreCommand());
            AnswerCommand = new Command(ExecuteAnswerCommand);
        }
        private async void ExecuteAnswerCommand(object obj)
        {
            var item = obj as Post;
            await Shell.Current.GoToAsync($"{nameof(AnswerPage)}?{nameof(AnswerViewModel.ItemId)}={item.Id}");
        }
        private async Task ExecuteLoadPostItemsCommand()
        {
            IsBusy = true;
            try
            {
                PostItems.Clear();
                SourcePostItems.Clear();
                
                var items = await postManager.GetItemsWithUid(LogedUser);
                foreach (var item in items)
                {
                    item.TitleInfo = await SetPostTitleInfo(item.Info);
                    PostItems.Add(item);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task<string> SetPostTitleInfo(PostInfo info)
        {
            if (info == null)
                return await Task.FromResult(string.Empty);
            string title = string.Empty;

            var chronology = info.Chronology;
            title = $"{info.Brand_Name} {info.Model_Name} {chronology}";
            return await Task.FromResult(title);
        }
        private async Task ExecuteLoadMoreCommand()
        {
            if (IsBusy)
                return;
            if (IsRefresing)
                return;
            try
            {
                IsRefresing = true;
                SourcePostItems.Clear();
                var items = await postManager.GetMoreItemsWithUid(LogedUser);
                foreach (var item in items)
                {
                    item.TitleInfo = await SetPostTitleInfo(item.Info);
                    PostItems.Add(item);
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefresing = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
