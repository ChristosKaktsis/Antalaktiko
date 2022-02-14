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
            foreach (var item in _viewModel.CommentCollection)
                BuildCommentUI(item.Id,item.Author_Name,item.Description,item.Date);
        }

        void BuildCommentUI(int id, string author, string description, string date)
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
                    horStack,answereLabel,answerEntry,answerButton
                }
            };
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