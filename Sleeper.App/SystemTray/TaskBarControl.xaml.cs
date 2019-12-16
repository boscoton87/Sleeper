using Sleeper.App.Controls;
using Sleeper.App.Interfaces;
using Sleeper.App.Models;
using Sleeper.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sleeper.App.SystemTray
{
    /// <summary>
    /// Interaction logic for TaskBarControl.xaml
    /// </summary>
    public partial class TaskBarControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public TaskBarControl()
        {
            DataContext = Facade.Services.Container.ResolveGlobalInstance<IAppSettingsContext>();

            var currentSettings = Facade.Services.Container.ResolveGlobalInstance<ISettingManager>().GetSettings();
            ((IAppSettingsContext)DataContext).AppSettings = BuildAppSettings(currentSettings);
            Facade.Services.Container.ResolveGlobalInstance<ISettingManager>().RegisterSettingChangeEmitter(
                settings =>
                {
                    ((IAppSettingsContext)DataContext).AppSettings.ForEach(setting =>
                    {
                        var menuItem = FindName(setting.ControlName) as MenuItem;
                        menuItem.IsChecked = bool.Parse(settings[setting.SettingName]);
                    });
                }
            );
            InitializeComponent();
            ((IAppSettingsContext)DataContext).AppSettings.ForEach(setting =>
            {
                var menuItem = FindName(setting.ControlName) as MenuItem;
                menuItem.IsEnabled = setting.IsEnabled;
                menuItem.IsChecked = bool.Parse(currentSettings[setting.SettingName]);
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
                },
                new AppSetting()
                {
                    SettingName = "modernStandbyEnabled",
                    ControlName = "ModernStandbyEnabled",
                    IsEnabled = true
                }
            };
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            var menuItem = e.Source as MenuItem;
            var appSetting = ((IAppSettingsContext)DataContext).AppSettings.Single(entry => entry.ControlName == menuItem.Name);
            Facade.Services.Container.ResolveGlobalInstance<ISettingManager>().UpdateSetting(appSetting.SettingName, menuItem.IsChecked.ToString());
            Facade.Services.Container.ResolveGlobalInstance<ISettingManager>().ApplySettings();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void ExitApplication(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
