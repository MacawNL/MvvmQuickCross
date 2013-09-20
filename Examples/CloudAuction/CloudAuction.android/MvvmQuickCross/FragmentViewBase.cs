using Android.App;
using Android.Views;

namespace MvvmQuickCross
{
    public class FragmentViewBase<ViewModelType> : Fragment, ViewDataBindings.ViewExtensionPoints where ViewModelType : ViewModelBase
    {
        private bool areHandlersAdded;
        protected ViewModelType ViewModel { get; private set; }
        protected ViewDataBindings Bindings { get; private set; }

        protected void Initialize(View rootView, ViewModelType viewModel, LayoutInflater layoutInflater, BindingParameters[] bindingsParameters = null, string idPrefix = null)
        {
            Bindings = new ViewDataBindings(rootView, viewModel, layoutInflater, idPrefix ?? this.GetType().Name + "_");
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

        public override void OnPause()
        {
            EnsureHandlersAreRemoved();
            base.OnPause();
        }

        public override void OnResume()
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

        public virtual void UpdateView(View view, object value)
        {
            ViewDataBindings.UpdateView(view, value);
        }
    }
}