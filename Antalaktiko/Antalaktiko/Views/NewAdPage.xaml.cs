using Antalaktiko.Services;
using Antalaktiko.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Antalaktiko.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAdPage : ContentPage
    {
        private NewAdViewModel _viewModel;
        private List<Grid> gridForRemove { get; set; }

        public NewAdPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new NewAdViewModel();
            gridForRemove = new List<Grid>();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            RemovePhotoButton.IsVisible = false;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            brandspopup.IsOpen = true;
        }

        private void ClosePopup_Clicked(object sender, EventArgs e)
        {
            brandspopup.IsOpen = false;
        }
        private void SearchText_TextChanged(object sender, EventArgs e)
        {
            BrandCollectionView.FilterString = "Contains([Name], '" + SearchText.Text + "')";
        }

        private void Models_Button_Clicked(object sender, EventArgs e)
        {
            modelspopup.IsOpen = true;
        }
        private void CloseModelsPopUp_Clicked(object sender, EventArgs e)
        {
            modelspopup.IsOpen = false;
        }
        private void SearchTextModels_TextChanged(object sender, EventArgs e)
        {
            ModelsCollectionView.FilterString = "Contains([Name], '" + SearchTextModels.Text + "')";
        }

       
        //Stopwatch stopwatch = new Stopwatch();
        

        private void ImageDeleteButton(object sender, EventArgs e)
        {
            //stopwatch.Start();

            //var stack = (sender as Button).Parent;
            //var imgToRemove = (stack.Parent as Grid);
            //ImageLayout.Children.Remove(imgToRemove);

            //Console.WriteLine("Delete Image");
            var button = (sender as Button);
            var setColor = button.BackgroundColor == Color.Transparent ? Color.FromHex("#b3a0a0a0") : Color.Transparent;
            var par = button.Parent as Grid;
            //add selected item for remove 
            if (gridForRemove.Contains(par))
                gridForRemove.Remove(par);
            else
                gridForRemove.Add(par);

            //Open Delete Button if any item
            if (gridForRemove.Any())
                RemovePhotoButton.IsVisible = true;
            else
                RemovePhotoButton.IsVisible = false;

            (sender as Button).BackgroundColor = setColor;
        }
        //private void ImageReleasedDelete(object sender, EventArgs e)
        //{
        //    //if (stopwatch.Elapsed.TotalSeconds >= 1)
        //    //{
        //    //    (sender as Button).Pressed -= ImageDeleteButton;
        //    //    (sender as Button).Released -= ImageReleasedDelete;
        //    //    var grid = (sender as Button).Parent;
        //    //    //var imgToRemove = (grid as Grid);
        //    //    //ImageLayout.Children.Remove(imgToRemove);
                
        //    //    Console.WriteLine("Delete Image");
        //    //}
        //    //stopwatch.Reset();
        //    //stopwatch.Stop();
        //}

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            foreach(var item in gridForRemove)
            {
                ImageLayout.Children.Remove(item);
            }
                
            gridForRemove.Clear();

            if (!gridForRemove.Any())
                RemovePhotoButton.IsVisible = false;
        }
        private async void PickPhotoButton_Clicked(object sender, EventArgs e)
        {

            // (sender as Button).IsEnabled = false;

            // Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            // if (stream != null)
            // {
            //     var imageSource = ImageSource.FromStream(() => stream);
            //     AddImageToLayout(imageSource);
            // }
            //(sender as Button).IsEnabled = true;
            await _viewModel.PickPhotoAsync();
            AddImageToLayout(_viewModel.ImageSource);
        }
        private async void TakePhotoButton_Clicked(object sender, EventArgs e)
        {
            await _viewModel.TakePhotoAsync();
            AddImageToLayout(_viewModel.ImageSource);
        }

        private void AddImageToLayout(ImageSource imageSource)
        {
            if (imageSource == null)
                return;
            var image = new Image { WidthRequest = 80, HeightRequest = 80 };
            var grid = new Grid { WidthRequest = 80, HeightRequest = 80, Margin = 10 };

            var button = new Button
            {
                BackgroundColor = Color.Transparent,
            };

            button.Clicked += ImageDeleteButton;
            image.Source = imageSource;
            grid.Children.Add(image);

            grid.Children.Add(button);
            ImageLayout.Children.Add(grid);

            //add image to image source 
           
            
        }
    }
}