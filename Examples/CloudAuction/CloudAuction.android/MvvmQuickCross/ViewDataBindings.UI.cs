using System;
using System.Collections;

using Android.Views;
using Android.Widget;

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
                case "Android.Widget.Button":
                    ((Button)binding.View).Click += Button_Click;
                    break;
                case "Android.Widget.Spinner":
                    ((Spinner)binding.View).ItemClick += Spinner_ItemClick;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void RemoveCommandHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.Button":
                    ((Button)binding.View).Click -= Button_Click;
                    break;
                case "Android.Widget.Spinner":
                    ((Spinner)binding.View).ItemClick -= Spinner_ItemClick;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var view = (Button)sender;
            var binding = FindBindingForView(view);
            if (binding != null)
            {
                var command = (RelayCommand)binding.ViewModelPropertyInfo.GetValue(viewModel);
                command.Execute(null);
            }
        }

        private void Spinner_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var spinner = (Spinner)sender;
            var binding = FindBindingForView(spinner);
            if (binding != null && spinner.Adapter is IDataBindableListAdapter)
            {
                var adapter = (IDataBindableListAdapter)spinner.Adapter;
                var command = (RelayCommand)binding.ViewModelPropertyInfo.GetValue(viewModel);
                command.Execute(adapter.GetItemAsObject(e.Position));
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
                        var spinner = (Spinner)view;
                        var adapter = spinner.Adapter as IDataBindableListAdapter;
                        if (adapter != null)
                        {
                            int position = adapter.GetItemPosition(value);
                            if (spinner.SelectedItemPosition != position) spinner.SetSelection(position);
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
                case "Android.Widget.EditText":
                    ((EditText)binding.View).AfterTextChanged += EditText_AfterTextChanged;
                    break;
                case "Android.Widget.Spinner":
                    var spinner = (Spinner)binding.View;
                    spinner.ItemSelected += Spinner_ItemSelected;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        private void RemoveTwoWayHandler(DataBinding binding)
        {
            if (binding.View == null) return;
            string viewTypeName = binding.View.GetType().FullName;
            switch (viewTypeName)
            {
                case "Android.Widget.EditText":
                    ((EditText)binding.View).AfterTextChanged -= EditText_AfterTextChanged;
                    break;
                case "Android.Widget.Spinner":
                    var spinner = (Spinner)binding.View;
                    spinner.ItemSelected -= Spinner_ItemSelected;
                    break;
                default:
                    throw new NotImplementedException("View type not implemented: " + viewTypeName);
            }
        }

        void EditText_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            var view = (EditText)sender;
            var binding = FindBindingForView(view);
            if (binding != null)
            {
                binding.ViewModelPropertyInfo.SetValue(viewModel, view.Text);
            }
        }

        void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (e.Position >= 0)
            {
                var view = (Spinner)sender;
                var binding = FindBindingForView(view);
                if (binding != null)
                {
                    var adapter = (IDataBindableListAdapter)view.Adapter;
                    binding.ViewModelPropertyInfo.SetValue(viewModel, adapter.GetItemAsObject(e.Position));
                }
            }
        }

        #endregion View types that support two-way data binding
    }
}