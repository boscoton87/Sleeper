using Sleeper.Taskbar.Models;
using System.Collections.Generic;

namespace Sleeper.Taskbar.Interfaces
{
    public interface IAppSettingsContext
    {
        List<AppSetting> AppSettings { get; set; }
    }
}
