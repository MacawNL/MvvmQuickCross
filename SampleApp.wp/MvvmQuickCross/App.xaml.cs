using System.Windows;
using System.Windows.Controls;
using SampleApp.Shared;

namespace SampleApp
{
    public partial class App : Application
    {
        public static SampleAppApplication EnsureSampleAppApplication(Frame navigationContext)
        {
            return SampleAppApplication.Instance ?? new SampleAppApplication(new SampleAppNavigator(), navigationContext);
        }

        // TODO: Add the following code to Application_Launching:
        //    EnsureSampleAppApplication(RootFrame).ContinueToMain(skipNavigation: true);

        // TODO: Add the following code to Application_Activated:
        //    EnsureSampleAppApplication(RootFrame);
    }
}
