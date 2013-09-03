using System.ComponentModel;
using System.Collections.Generic;

namespace MvvmQuickCross
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
#if __ANDROID__ || __IOS__
        private HashSet<string> changedProperties = new HashSet<string>();

        private PropertyChangedEventHandler _propertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _propertyChanged += value;
                if (changedProperties.Count > 0)
                {
                    foreach (string propertyName in changedProperties) RaisePropertyChanged(propertyName);
                    changedProperties.Clear();
                }
            }

            remove { _propertyChanged -= value; }
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = _propertyChanged;
            if (handler != null)
            {
                ApplicationBase.RunOnUIThread(() => handler(this, new PropertyChangedEventArgs(propertyName)));
            }
            else
            {
                changedProperties.Add(propertyName);
            }
        }
#else
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) ApplicationBase.RunOnUIThread(() => handler(this, new PropertyChangedEventArgs(propertyName)));
        }
#endif
    }
}
