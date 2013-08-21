using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmQuickCross
{
    public abstract class ApplicationBase
    {
        private static object _syncRoot = new Object();
        private static volatile ApplicationBase _current;
        private TaskScheduler _uiTaskScheduler;

        /// <summary>
        /// Initializes the application singleton and the UI thread scheduler
        /// </summary>
        /// <param name="uiTaskScheduler">Optional. If omitted, ensure that the application is always created on the UI thread.</param>
        protected ApplicationBase(TaskScheduler uiTaskScheduler = null)
        {
            lock (_syncRoot) 
            {
                if (_current != null) throw new InvalidOperationException("No more than one instance of ApplicationBase may be created.");
                _current = this;
            }

            _uiTaskScheduler = (uiTaskScheduler != null) ? uiTaskScheduler : TaskScheduler.FromCurrentSynchronizationContext();
            // TODO: add platform-specific check that _uiTaskScheduler can access the UI thread - e.g. try something that requires UI acces and catch the exception, rethrow with clear error message.
        }

        protected static ApplicationBase Current { get { return _current; } }

        public static Task RunOnUIThread(Action action)
        {
            return (Current == null) ?
                       Task.Factory.StartNew(action) : // This is intended for a runtime context where no Application instance exists and we are already in a UI thread - i.e. when code is run at design-time in Visual Studio
                       Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, Current._uiTaskScheduler);
        }

        public object CurrentNavigationContext { get; set; }
    }
}
