using System;
using System.Windows.Controls;
using SampleApp.Shared;

namespace SampleApp
{
    class SampleAppNavigator : ISampleAppNavigator
    {
        private void Navigate(object navigationContext, string uri)
        {
            ((Frame)navigationContext).Navigate(new Uri(uri, UriKind.Relative));
        }

        public void NavigateToSampleItemListView(object navigationContext)
        {
            Navigate(navigationContext, "/SampleItemListView.xaml");
        }

        public void NavigateToSampleItemView(object navigationContext)
        {
            Navigate(navigationContext, "/SampleItemView.xaml");
        }
    }
}
