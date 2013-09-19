using System;
using System.Collections;

using Android.Views;
using Android.Widget;
using System.Reflection;

namespace MvvmQuickCross
{
    public partial class ViewDataBindings
    {
        #region View types that support command binding

        private void AddCommandHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.Button": binding.View.Click += View_Click; break;
                case "Android.Widget.Spinner": ((AdapterView)binding.View).ItemSelected += AdapterView_ItemSelected; break;
                case "Android.Widget.ListView": ((AdapterView)binding.View).ItemClick += AdapterView_ItemClick; break;
                default: throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void RemoveCommandHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.Button": binding.View.Click -= View_Click; break;
                case "Android.Widget.Spinner": ((AdapterView)binding.View).ItemSelected -= AdapterView_ItemSelected; break;
                case "Android.Widget.ListView": ((AdapterView)binding.View).ItemClick -= AdapterView_ItemClick; break;
                default: throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void View_Click(object sender, EventArgs e)
        {
            var view = (Button)sender;
            var binding = FindBindingForView(view);
            if (binding != null)
            {
                var command = (RelayCommand)binding.ViewModelPropertyInfo.GetValue(viewModel);
                command.Execute(null);
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
                    case "Android.Widget.TextView":
                    case "Android.Widget.EditText":
                        var textView = (TextView)view;
                        string text = value.ToString();
                        if (textView.Text != text) textView.Text = text;
                        break;

                    case "Android.Widget.ProgressBar":
                        ((ProgressBar)view).Progress = (int)value;
                        break;

                    case "Android.Widget.Spinner":
                    case "Android.Widget.ListView":
                        var adapterView = (AdapterView)view;
                        var adapter = adapterView.GetAdapter() as IDataBindableListAdapter;
                        if (adapter != null)
                        {
                            int position = adapter.GetItemPosition(value);
                            if (adapterView.SelectedItemPosition != position) adapterView.SetSelection(position);
                        }
                        break;

                    case "Macaw.UIComponents.MultiImageView":
                        if (value is Uri) value = ((Uri)value).AbsoluteUri;
                        var multiImageView = (Macaw.UIComponents.MultiImageView)view;
                        multiImageView.LoadImageList(new[] { (string)value });
                        multiImageView.LoadImage(); // TODO: Update to MultiImageView 1.2 when it is published, to fix hang on no connection and to get rid of this LoadImage call.
                        break;

                    default:
                        throw new NotImplementedException("View type not implemented: " + viewTypeName);
                }
            }
        }

        #endregion View types that support one-way data binding

        #region View types that support two-way data binding

        private void AddTwoWayHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.EditText": ((TextView)binding.View).AfterTextChanged += TextView_AfterTextChanged; break;
                case "Android.Widget.Spinner":  ((AdapterView)binding.View).ItemSelected += AdapterView_ItemSelected; break;
                case "Android.Widget.ListView": ((AdapterView)binding.View).ItemClick += AdapterView_ItemClick; break;
                default: throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void RemoveTwoWayHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.EditText": ((TextView)binding.View).AfterTextChanged -= TextView_AfterTextChanged; break;
                case "Android.Widget.Spinner": ((AdapterView)binding.View).ItemSelected -= AdapterView_ItemSelected; break;
                case "Android.Widget.ListView": ((AdapterView)binding.View).ItemClick -= AdapterView_ItemClick; break;
                default: throw new NotImplementedException("View type not implemented: " + viewTypeName);
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