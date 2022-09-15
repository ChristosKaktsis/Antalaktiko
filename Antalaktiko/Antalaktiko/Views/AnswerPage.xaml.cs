using Antalaktiko.ViewModels;
using DevExpress.XamarinForms.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Antalaktiko.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnswerPage : ContentPage
    {
        private AnswerViewModel _viewModel;

        public AnswerPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AnswerViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadComments();
            LoadCommentsInUI();
            
        }

        private void LoadCommentsInUI()
        {
            CommentStack.Children.Clear();
            //foreach (var item in _viewModel.CommentCollection)
            //    BuildCommentUI(item.Id,item.Author_Name,item.Description,item.Date,item.Author.ToString()==App.LogedUser.Id);
           
            foreach (var item in _viewModel.CommentCollection)
                BuildCommentUI2(item);

        }

        private void BuildCommentUI2(Models.Comment item)
        {
            //do it only for top level coments
            if (item.Parent != 0)
                return;

            Frame frame = new Frame
            {
                Padding = 10,
                Margin = 5,
                CornerRadius = 5

            };

            StackLayout commentStack = CreateCommentStack(item);
            //if its this.user comment dont answer

            frame.Content = commentStack;
            CommentStack.Children.Add(frame);
        }

        private StackLayout CreateCommentStack(Models.Comment item)
        {
            if (item == null)
                return new StackLayout();
            Label companyLabel = new Label
            {
                Text = item.Author_Name,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            Label dateLabel = new Label
            {
                Text = item.Date,

            };
            StackLayout horStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    companyLabel,dateLabel
                }
            };
            Label answereLabel = new Label { Text = item.Description };
            MultilineEdit answerEntry = new MultilineEdit { PlaceholderText = "Απάντησε εδώ...", AutomationId = item.Id.ToString() };
            Button answerButton = new Button { Text = "Απάντησε", HorizontalOptions = LayoutOptions.End, CornerRadius = 10 };
            answerButton.Clicked += AnswerButton_Clicked;

            StackLayout stackLayout = new StackLayout
            {
                Children =
                {
                    horStack,answereLabel
                }
            };
            var isUsers = item.Author.ToString() == App.LogedUser.Id;
            //if its this.user comment dont answer
            if (!isUsers && item.Child==null)
            {
                stackLayout.Children.Add(answerEntry);
                stackLayout.Children.Add(answerButton);
            }
            stackLayout.Children.Add(CreateCommentStack(item.Child));
            return stackLayout;
        }

        void BuildCommentUI(int id, string author, string description, string date, bool isUsers)
        {
            Frame frame = new Frame
            {
                Padding = 10,
                Margin = 5,
                CornerRadius = 5
               
            };
           
            Label companyLabel = new Label
            {
                Text = author,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            Label dateLabel = new Label 
            { 
                Text = date,
                
            };
            StackLayout horStack = new StackLayout 
            { 
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    companyLabel,dateLabel
                }
            };
            Label answereLabel = new Label { Text = description};
            MultilineEdit answerEntry = new MultilineEdit { PlaceholderText = "Απάντησε εδώ...", AutomationId = id.ToString()};
            Button answerButton = new Button { Text = "Απάντησε", HorizontalOptions = LayoutOptions.End, CornerRadius = 10 };
            answerButton.Clicked += AnswerButton_Clicked;

            StackLayout stackLayout = new StackLayout
            {
                Children =
                {
                    horStack,answereLabel
                }
            };

            //if its this.user comment dont answer
            if (!isUsers)
            {
                stackLayout.Children.Add(answerEntry);
                stackLayout.Children.Add(answerButton);
            }

            frame.Content = stackLayout;
            CommentStack.Children.Add(frame);
        }

        private async void AnswerButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var stack = button.Parent as StackLayout;
           
           
            //get the id and the descr
            string comId = string.Empty;
            string desc = string.Empty;
            foreach(var item in stack.Children)
            {
                if(item is MultilineEdit)
                {
                    comId = (item as MultilineEdit).AutomationId ;
                    desc = (item as MultilineEdit).Text;
                }
            }
            if (string.IsNullOrWhiteSpace(desc))
                return;
            stack.Children.Add(new Label { Text = "Απάντήθηκε !!" });
            //lock the UI
            button.IsVisible = false;
            stack.IsEnabled = false;
            //send to service
            await _viewModel.AnswerComment(comId, desc);
            //unsub the event 
            button.Clicked -= AnswerButton_Clicked;
        }

        //private async void AnswerButton_Clicked(object sender, EventArgs e)
        //{
        //    await _viewModel.AnswerComment();
        //    await _viewModel.LoadComments();
        //    LoadCommentsInUI();
        //}
    }
}