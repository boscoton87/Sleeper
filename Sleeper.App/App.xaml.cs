using Facade.Services;
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
            Container.RegisterGlobalInstance<ISettingLoader>(GetSettings());
            Container.RegisterGlobalInstance<IAppSettingsContext>(new AppSettingsContext());
        }

        private SettingLoader GetSettings()
        {
            var programDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Sleeper");
            var settingsFilePath = Path.Combine(programDataPath, "settings.json");
            if (!File.Exists(settingsFilePath))
            {
                File.WriteAllText(settingsFilePath, JsonConvert.SerializeObject(new Dictionary<string, string>()));
            }
            Directory.CreateDirectory(programDataPath);
            var settingLoader = new SettingLoader();
            settingLoader.RegisterSettingMapping(
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
                        string isAdminValue = Container.ResolveGlobalInstance<ISettingLoader>().GetSettings()["isAdmin"];
                        if (bool.TryParse(value, out isSet) && bool.TryParse(isAdminValue, out isAdmin) && isAdmin)
                        {
                            WindowsSystemHelpers.SetHibernate(isSet);
                        }
                    }
                }
            );
            settingLoader.RegisterSettingMapping(
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
            settingLoader.RegisterSettingMapping(
                "modernStandbyEnabled",
                new SettingMapping()
                {
                    Load = () =>
                    {
                        var fileContents = File.ReadAllText(settingsFilePath);
                        var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContents);
                        return settings.ContainsKey("modernStandbyEnabled") ? settings["modernStandbyEnabled"] : bool.FalseString;
                    },
                    Apply = (value) =>
                    {
                        var fileContents = File.ReadAllText(settingsFilePath);
                        var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileContents);
                        if (settings.ContainsKey("modernStandbyEnabled"))
                        {
                            settings["modernStandbyEnabled"] = value;
                        } else
                        {
                            settings.Add("modernStandbyEnabled", value);
                        }
                        File.WriteAllText(settingsFilePath, JsonConvert.SerializeObject(settings));
                    }
                }
            );
            settingLoader.GatherSettings();
            return settingLoader;
        }
    }
}
