using Antalaktiko.Views;
using System;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    class StartViewModel : BaseViewModel
    {
        public Command NewAdCommand { get; }
        public StartViewModel()
        {
            NewAdCommand = new Command(OnNewAdPressed);
        }

        private async void OnNewAdPressed(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewAdPage));
        }
    }
}
