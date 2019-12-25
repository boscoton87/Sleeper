using Sleeper.Taskbar.Interfaces;
using System.Collections.Generic;

namespace Sleeper.Taskbar.Models
{
    public class AppSettingsContext : IAppSettingsContext
    {
        public List<AppSetting> AppSettings { get; set; } = new List<AppSetting>();
    }
}
