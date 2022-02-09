using Antalaktiko.Models;
using Antalaktiko.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Antalaktiko.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        public IDataManger<Brand> brandManager = new BrandManager();
        public IDataManger<Part> partManager = new PartManager();
        public CompanyManager companyManager = new CompanyManager();
        public PostManger postManager = new PostManger();
        public UserManager userManger = new UserManager();
        public CommentManager commentManager = new CommentManager();
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        public bool IsRefresing
        {
            get { return isRefresing; }
            set { SetProperty(ref isRefresing, value); }
        }
        string title = string.Empty;
        private bool isRefresing;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        string logedUser;
        public string LogedUser
        {
            get
            {
                if (App.LogedUser == null)
                    return string.Empty;

                logedUser = App.LogedUser.Id;
                return logedUser;
            }
        }
        public async void OnGoBackClicked()
        {
            await Shell.Current.GoToAsync($"..");
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
