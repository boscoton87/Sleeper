using Sleeper.App.Interfaces;
using Sleeper.App.Models;
using Sleeper.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sleeper.App.Controls
{
    /// <summary>
    /// Interaction logic for AppSettingsPage.xaml
    /// </summary>
    public partial class AppSettingsPage : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AppSettingsPage()
        {
            DataContext = Facade.Services.Container.ResolveGlobalInstance<IAppSettingsContext>();
            
            var currentSettings = Facade.Services.Container.ResolveGlobalInstance<ISettingLoader>().GetSettings();
            ((IAppSettingsContext)DataContext).AppSettings = BuildAppSettings(currentSettings);
            Facade.Services.Container.ResolveGlobalInstance<ISettingLoader>().RegisterSettingChangeEmitter(
                settings =>
                {
                    ((IAppSettingsContext)DataContext).AppSettings.ForEach(setting => {
                        var childControl = FindName(setting.ControlName) as IEmitterControl;
                        childControl.SetState(settings[setting.SettingName]);
                    });
                }
            );
            InitializeComponent();
            ((IAppSettingsContext)DataContext).AppSettings.ForEach( setting => {
                var childControl = FindName(setting.ControlName) as IEmitterControl;
                childControl.AppSetting = setting;
                childControl.SetState(currentSettings[setting.SettingName]);
                childControl.SetEnabled(setting.IsEnabled);
                if (!setting.IsEnabled)
                {
                    var messageLabel = FindName($"{setting.ControlName}_LockMessage");
                    if(messageLabel != null)
                    {
                        ((Label)messageLabel).Content = setting.LockedMessage;
                    }
                }
            });
        }

        private List<AppSetting> BuildAppSettings(Dictionary<string, string> currentSettings)
        {
            return new List<AppSetting>()
            {
                new AppSetting()
                {
                    SettingName = "hibernateEnabled",
                    ControlName = "HibernateEnabled",
                    IsEnabled = bool.Parse(currentSettings["isAdmin"]),
                    LockedMessage = "Requires Admin Access"
                },
                new AppSetting()
                {
                    SettingName = "modernStandbyEnabled",
                    ControlName = "ModernStandbyEnabled",
                    IsEnabled = true
                }
            };
        }

        private void SaveSettings(object sender, MouseButtonEventArgs e)
        {
            ((IAppSettingsContext)DataContext).AppSettings.ForEach(setting =>
            {
                Facade.Services.Container.ResolveGlobalInstance<ISettingLoader>().UpdateSetting(setting.SettingName, setting.Value);
            });
            Facade.Services.Container.ResolveGlobalInstance<ISettingLoader>().ApplySettings();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
