using Android.App;
using Android.Views;

namespace MvvmQuickCross
{
    public class ActivityViewBase<ViewModelType> : Activity where ViewModelType : ViewModelBase
    {
        private bool areHandlersAdded;
        protected ViewModelType ViewModel { get; private set; }
        protected ViewDataBindings Bindings { get; private set; }

        protected void Initialize(View rootView, ViewModelType viewModel, BindingParameters[] bindingsParameters = null, string idPrefix = null)
        {
            ApplicationBase.Instance.CurrentNavigationContext = this;
            Bindings = new ViewDataBindings(rootView, viewModel, idPrefix ?? this.GetType().Name);
            ViewModel = viewModel;
            EnsureHandlersAreAdded();
            Bindings.AddBindings(bindingsParameters); // First add any bindings that were specified in code 
            Bindings.EnsureCommandBindings();  // Then add any command bindings that were not specified in code (based on the Id naming convention)
            ViewModel.RaisePropertiesChanged(); // Finally add any property bindings that were not specified in code (based on the Id naming convention), and update the root view with the current property values
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

        protected override void OnPause()
        {
            EnsureHandlersAreRemoved();
            base.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            EnsureHandlersAreAdded();
        }

        private void EnsureHandlersAreAdded()
        {
            if (areHandlersAdded) return;
            AddHandlers();
            areHandlersAdded = true;
        }

        private void EnsureHandlersAreRemoved()
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