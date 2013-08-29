using System.ComponentModel;

namespace MvvmQuickCross
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) ApplicationBase.RunOnUIThread(() => handler(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}
