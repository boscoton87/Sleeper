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
            
            var appSettings = Facade.Services.Container.ResolveGlobalInstance<ISettingLoader>().GetSettings();
            ((IAppSettingsContext)DataContext).ChildControlNames = new List<string>() { "HibernateEnabled" };
            ((IAppSettingsContext)DataContext).HibernateEnabledTextBox = bool.Parse(appSettings["hibernateEnabled"]) ? "Right" : "Left";
            ((IAppSettingsContext)DataContext).IsAdminTextBox = appSettings["isAdmin"];
            Facade.Services.Container.ResolveGlobalInstance<ISettingLoader>().RegisterSettingChangeEmitter(
                settings =>
                {
                    ((IAppSettingsContext)DataContext).HibernateEnabledTextBox = bool.Parse(appSettings["hibernateEnabled"]) ? "Right" : "Left";
                    OnPropertyChanged("HibernateEnabled");
                    ((IAppSettingsContext)DataContext).IsAdminTextBox = appSettings["isAdmin"];
                    OnPropertyChanged("IsAdmin");
                }
            );
            ((IAppSettingsContext)DataContext).ChildChangeEmitters.Add("HibernateEnabled", new List<Action<string>>()
            {
                value =>
                {
                    ((IAppSettingsContext)DataContext).HibernateEnabledTextBox = value;
                }
            });
            InitializeComponent();
            ((IAppSettingsContext)DataContext).ChildControlNames.ForEach( childName => {
                var childControl = FindName(childName) as IEmitterControl;
                childControl.ChangeEmitters = ((IAppSettingsContext)DataContext).ChildChangeEmitters[childName];
                childControl.SetState(((IAppSettingsContext)DataContext).HibernateEnabledTextBox);
            });
        }

        private void SaveSettings(object sender, MouseButtonEventArgs e)
        {
            Facade.Services.Container.ResolveGlobalInstance<ISettingLoader>().UpdateSetting("hibernateEnabled", (((IAppSettingsContext)DataContext).HibernateEnabledTextBox == "Right").ToString());
            Facade.Services.Container.ResolveGlobalInstance<ISettingLoader>().ApplySettings();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

public class AppSetting
{
    public string ControlName { get; set; }

    public string PropertyName { get; set; }

    public string SettingName { get; set; }

    public Func<object, string> ConvertFromSettingToValue { get; set; }

    public Func<string, object> ConvertFromValueToSetting { get; set; }

    public List<Action<string>> ChangeEmitters { get; set; }
}
