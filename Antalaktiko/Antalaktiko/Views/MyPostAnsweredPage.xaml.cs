using Antalaktiko.ViewModels;
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
    public partial class MyPostAnsweredPage : ContentPage
    {
        private MyPostAnsweredViewModel _viewModel;

        public MyPostAnsweredPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MyPostAnsweredViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;

            _viewModel.AnswerCommand.Execute(item.BindingContext);
        }
    }
}