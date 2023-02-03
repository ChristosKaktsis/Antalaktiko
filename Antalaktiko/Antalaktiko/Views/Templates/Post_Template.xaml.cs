using Antalaktiko.Models;
using Antalaktiko.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Antalaktiko.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Post_Template : ContentView
    {
        public Post_Template()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            LoadBitmapCollection();
            //set the state label value cause state string is a list oops
            SetStateLable();
        }

        private void SetStateLable()
        {
            if(!(BindingContext is Post post)) return;
            string value = string.Empty;
            post?.Data?.State_string.ForEach(item =>
            {
                value += item;
            });
            state_label.Text = value;
        }

        void LoadBitmapCollection()
        {
            try
            {
                flexLayout.Children.Clear();
                //await Task.Delay(100);
                var item = this.BindingContext as Post;
                if (item is null) return;
                // Create an Image object for each bitmap
                if (item.Data.Images is null) return;
                foreach (var i in item.Data.Images)
                {
                    Xamarin.Forms.Image image = new Xamarin.Forms.Image
                    {
                        Source = ImageSource.FromUri(new Uri(i.Thumb)),
                        HeightRequest= 50,WidthRequest= 50,
                        Margin = 5
                    };
                    flexLayout.Children.Add(image);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var item = this.BindingContext as Post;
            await Shell.Current.GoToAsync($"{nameof(AnswerPage)}?{nameof(AnswerViewModel.ItemId)}={item.Id}");
        }
    }
}