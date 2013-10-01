using System;

using Android.Views;
using Android.Widget;

namespace MvvmQuickCross
{
    public partial class ViewDataBindings
    {
        #region View types that support command binding

        private void AddCommandHandler(DataBinding binding)
        {
            var view = binding.View;
            if (view == null) return;
            string viewTypeName = view.GetType().FullName;
            switch (viewTypeName)
            {
                // TODO: Add cases here for specialized view types, as needed
                default:
                    if (view is AbsSpinner) ((AdapterView)view).ItemSelected += AdapterView_ItemSelected;
                    else if (view is AdapterView) ((AdapterView)view).ItemClick += AdapterView_ItemClick;
                    else view.Click += View_Click;
                    break;
            }
        }

        private void RemoveCommandHandler(DataBinding binding)
        {
            var view = binding.View;
            if (view == null) return;
            string viewTypeName = view.GetType().FullName;
            switch (viewTypeName)
            {
                // TODO: Add cases here for specialized view types, as needed
                default:
                    if (view is AbsSpinner) ((AdapterView)view).ItemSelected -= AdapterView_ItemSelected;
                    else if (view is AdapterView) ((AdapterView)view).ItemClick -= AdapterView_ItemClick;
                    else view.Click -= View_Click;
                    break;
            }
        }

        private void View_Click(object sender, EventArgs e)
        {
            var view = (View)sender;
            var binding = FindBindingForView(view);
            if (binding != null)
            {
                var command = (RelayCommand)binding.ViewModelPropertyInfo.GetValue(viewModel);
                object parameter = null;
                if (binding.CommandParameterSelectedItemAdapterView != null)
                {
                    var adapter = binding.CommandParameterSelectedItemAdapterView.GetAdapter() as IDataBindableListAdapter;
                    var adapterView = binding.CommandParameterSelectedItemAdapterView;
                    int selectedItemPosition = (adapterView is AbsListView) ? ((AbsListView)adapterView).CheckedItemPosition : adapterView.SelectedItemPosition;
                    if (adapter != null && selectedItemPosition >= 0) parameter = adapter.GetItemAsObject(selectedItemPosition);
                }
                command.Execute(parameter);
            }
        }

        #endregion View types that support command binding

        #region View types that support one-way data binding

        public static void UpdateView(View view, object value)
        {
            if (view != null)
            {
                string viewTypeName = view.GetType().FullName;
                switch (viewTypeName)
                {
                    // TODO: Add cases here for specialized view types, as needed
                    case "Android.Widget.ProgressBar":
                        {
                            var progressBar = (ProgressBar)view;
                            int progressValue = (int)(value ?? 0);
                            if (progressBar.Progress != progressValue) progressBar.Progress = progressValue;
                        }
                        break;

                    case "Android.Webkit.WebView":
                        {
                            var webView = (Android.Webkit.WebView)view;
                            if (value is Uri)
                            {
                                string newUrl = value.ToString();
                                if (webView.Url != newUrl) webView.LoadUrl(newUrl);
                            }
                            else
                            {
                                webView.LoadData(value == null ? "" : value.ToString(), "text/html", null);
                            }
                        }
                        break;

                    default:
                        if (view is TextView)
                        {
                            var textView = (TextView)view;
                            string text = value == null ? "" : value.ToString();
                            if (textView.Text != text) textView.Text = text;
                        }
                        else if (view is AdapterView)
                        {
                            var adapterView = (AdapterView)view;
                            var adapter = adapterView.GetAdapter() as IDataBindableListAdapter;
                            if (adapter != null)
                            {
                                int position = adapter.GetItemPosition(value);
                                if (adapterView.SelectedItemPosition != position) adapterView.SetSelection(position);
                            }
                        }
                        else throw new NotImplementedException("View type not implemented: " + viewTypeName);
                        break;
                }
            }
        }

        #endregion View types that support one-way data binding

        #region View types that support two-way data binding

        private void AddTwoWayHandler(DataBinding binding)
        {
            var view = binding.View;
            if (view == null) return;
            string viewTypeName = view.GetType().FullName;
            switch (viewTypeName)
            {
                // TODO: Add cases here for specialized view types, as needed
                default:
                    if (view is AbsSpinner) ((AdapterView)view).ItemSelected += AdapterView_ItemSelected;
                    else if (view is AbsListView) ((AdapterView)view).ItemClick += AdapterView_ItemClick;
                    else if (view is EditText) ((TextView)view).AfterTextChanged += TextView_AfterTextChanged;
                    else throw new NotImplementedException("View type not implemented: " + viewTypeName);
                    break;
            }
        }

        private void RemoveTwoWayHandler(DataBinding binding)
        {
            var view = binding.View;
            if (view == null) return;
            string viewTypeName = view.GetType().FullName;
            switch (viewTypeName)
            {
                // TODO: Add cases here for specialized view types, as needed
                default:
                    if (view is AbsSpinner) ((AdapterView)view).ItemSelected -= AdapterView_ItemSelected;
                    else if (view is AbsListView) ((AdapterView)view).ItemClick -= AdapterView_ItemClick;
                    else if (view is EditText) ((TextView)view).AfterTextChanged -= TextView_AfterTextChanged;
                    else throw new NotImplementedException("View type not implemented: " + viewTypeName);
                    break;
            }
        }

        private void TextView_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            var view = (TextView)sender;
            var binding = FindBindingForView(view);
            if (binding != null)
            {
                binding.ViewModelPropertyInfo.SetValue(viewModel, view.Text);
            }
        }

        private void HandleAdapterViewItemChosen(AdapterView adapterView, int itemPosition)
        {
            if (itemPosition >= 0)
            {
                var adapter = adapterView.GetAdapter() as IDataBindableListAdapter;
                var binding = FindBindingForView(adapterView);
                if (adapter != null && binding != null)
                {
                    var value = adapter.GetItemAsObject(itemPosition);
                    switch (binding.Mode)
                    {
                        case BindingMode.Command:
                            var command = (RelayCommand)binding.ViewModelPropertyInfo.GetValue(viewModel);
                            command.Execute(value);
                            break;
                        case BindingMode.TwoWay:
                            binding.ViewModelPropertyInfo.SetValue(viewModel, value);
                            break;
                    }
                }
            }
        }

        private void AdapterView_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            HandleAdapterViewItemChosen((AdapterView)sender, e.Position);
        }

        private void AdapterView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            HandleAdapterViewItemChosen((AdapterView)sender, e.Position);
        }

        #endregion View types that support two-way data binding
    }
}