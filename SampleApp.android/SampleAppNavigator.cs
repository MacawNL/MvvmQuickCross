using System;

using Android.Content;
using SampleApp.Shared;


namespace SampleApp
{
    class SampleAppNavigator : ISampleAppNavigator
    {
        private void Navigate(object navigationContext, Type type)
        {
            var context = (Context)navigationContext;
            context.StartActivity(type);
        }

        public void NavigateToSampleItemListView(object navigationContext)
        {
            Navigate(navigationContext, typeof(SampleItemListView));
        }

        public void NavigateToSampleItemView(object navigationContext)
        {
            Navigate(navigationContext, typeof(SampleItemView));
        }
    }
}
