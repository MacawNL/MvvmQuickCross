using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;

namespace MvvmQuickCross
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
#if __ANDROID__ || __IOS__
        private List<string> propertyNames;

        public void NotifyAllPropertiesChanged()
        {
            if (propertyNames == null)
            {
                propertyNames = new List<string>();
                foreach (var fieldInfo in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                {
                    if (fieldInfo.IsLiteral && !fieldInfo.IsInitOnly && fieldInfo.Name.StartsWith("PROPERTYNAME_"))
                    {
                        propertyNames.Add((string)fieldInfo.GetValue(null));
                    }
                }
            }

            foreach (string propertyName in propertyNames) RaisePropertyChanged(propertyName);
        }
#endif
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) ApplicationBase.RunOnUIThread(() => handler(this, new PropertyChangedEventArgs(propertyName)));
        }
    }
}
