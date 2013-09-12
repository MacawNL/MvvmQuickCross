using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MvvmQuickCross
{
    public class ActivityViewBase<ViewModelType> : Activity where ViewModelType : ViewModelBase
    {
        private bool areHandlersAdded;
        protected ViewModelType ViewModel { get; private set; }
        protected ViewDataBindings Bindings { get; private set; }

        protected void Initialize(View view, ViewModelType viewModel, string idPrefix = null)
        {
            Bindings = new ViewDataBindings(view, viewModel, idPrefix ?? this.GetType().Name);
            ViewModel = viewModel;
            EnsureHandlersAreAdded();
            ViewModel.RaisePropertiesChanged();
        }

        protected virtual void AddHandlers()
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            Bindings.AddHandlers();
        }

        protected virtual void RemoveHandlers()
        {
            Bindings.RemoveHandlers();
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            Bindings.UpdateView(propertyName);
        }

        protected void EnsureHandlersAreAdded()
        {
            if (areHandlersAdded) return;
            AddHandlers();
            areHandlersAdded = true;
        }

        protected void EnsureHandlersAreRemoved()
        {
            if (!areHandlersAdded) return;
            RemoveHandlers();
            areHandlersAdded = false;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }
    }
}