using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private bool isPopUpOpen;

        public Command GoBackCommand { get; }
        public Command OpenPopUpCommand { get; }
        public RegisterViewModel()
        {
            GoBackCommand = new Command(OnGoBackClicked);
            OpenPopUpCommand = new Command(OnOpenPopUp);
        }

        private void OnOpenPopUp(object obj)
        {
            IsPopUpOpen = !IsPopUpOpen;
        }

        public bool IsPopUpOpen
        {
            get => isPopUpOpen;
            set => SetProperty(ref isPopUpOpen, value);
        }
    }
}
