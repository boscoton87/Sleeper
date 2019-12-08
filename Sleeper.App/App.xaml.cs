using Facade.Services;
using Hardcodet.Wpf.TaskbarNotification;
using Newtonsoft.Json;
using Sleeper.App.Interfaces;
using Sleeper.App.Models;
using Sleeper.Core.Helpers;
using Sleeper.Core.Interfaces;
using Sleeper.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
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

            var programDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Sleeper");
            Container.RegisterGlobalInstance<ISettingLoader>(new JsonSettingLoader(programDataPath));
            Container.RegisterGlobalInstance<ISettingManager>(GetSettings());
            Container.RegisterGlobalInstance<IAppSettingsContext>(new AppSettingsContext());


            InitializeComponent();
            Container.RegisterGlobalInstance<ITaskBar>(new TaskBar((TaskbarIcon)FindResource("SystemTrayIcon"), null));
        }

        private SettingManager GetSettings()
        {
            var settingLoader = Container.ResolveGlobalInstance<ISettingLoader>();
            var settingManager = new SettingManager();
            settingManager.RegisterSettingMapping(
                "hibernateEnabled",
                new SettingMapping()
                {
                    Load = () =>
                    {
                        return File.Exists(@"C:\hiberfil.sys").ToString();
                    },
                    Apply = (value) =>
                    {
                        bool isSet;
                        bool isAdmin;
                        string isAdminValue = Container.ResolveGlobalInstance<ISettingManager>().GetSettings()["isAdmin"];
                        if (bool.TryParse(value, out isSet) && bool.TryParse(isAdminValue, out isAdmin) && isAdmin)
                        {
                            WindowsSystemHelpers.SetHibernate(isSet);
                        }
                    }
                }
            );
            settingManager.RegisterSettingMapping(
                "isAdmin",
                new SettingMapping()
                {
                    Load = () =>
                    {
                        var identity = WindowsIdentity.GetCurrent();
                        var principal = new WindowsPrincipal(identity);
                        return principal.IsInRole(WindowsBuiltInRole.Administrator).ToString();
                    },
                    Apply = (value) => { }
                }
            );
            settingManager.RegisterSettingMapping(
                "modernStandbyEnabled",
                new SettingMapping()
                {
                    Load = () =>
                    {
                        return settingLoader.GetSetting("modernStandbyEnabled") ?? bool.FalseString;
                    },
                    Apply = (value) =>
                    {
                        settingLoader.ApplySetting("modernStandbyEnabled", value);
                    }
                }
            );
            settingManager.GatherSettings();
            return settingManager;
        }
    }
}
