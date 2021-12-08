using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WartungsLOG.Core
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isBusy;
        private string _title = string.Empty;
        

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T dest, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(dest, value))
            {
                return false;
            }

            dest = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        
        public bool IsBusy { get => _isBusy; set => SetProperty(ref _isBusy, value); }
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public virtual Task Initialize()
        {
            return Task.FromResult(default(object));
        }
    }
}
