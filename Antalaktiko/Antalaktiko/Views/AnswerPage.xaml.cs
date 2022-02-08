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
                BuildCommentUI(item.Author,item.Description,item.Date);
        }

        void BuildCommentUI(int author, string description, string date)
        {
            Frame frame = new Frame
            {
                Padding = 10,
                Margin = 5,
                CornerRadius = 5
               
            };
           
            Label companyLabel = new Label
            {
                Text = author.ToString(),
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
            StackLayout stackLayout = new StackLayout
            {
                Children =
                {
                    horStack,answereLabel
                }
            };
            frame.Content = stackLayout;
            CommentStack.Children.Add(frame);
        }

        private async void AnswerButton_Clicked(object sender, EventArgs e)
        {
            await _viewModel.AnswerComment();
            await _viewModel.LoadComments();
            LoadCommentsInUI();
        }
    }
}