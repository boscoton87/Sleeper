using Facade.Services;
using Sleeper.Core.Helpers;
using Sleeper.Core.Interfaces;
using Sleeper.Core.Services;
using System;
using System.Windows;

namespace Sleeper.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Action sleepAction = () => WindowsSystemHelpers.PerformSleep();
            Container.RegisterGlobalInstance<IDelayedActionService>(new DelayedActionService(sleepAction));
        }
    }
}
