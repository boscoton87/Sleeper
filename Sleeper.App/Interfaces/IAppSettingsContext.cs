using Sleeper.App.Controls;
using Sleeper.App.Models;
using System;
using System.Collections.Generic;

namespace Sleeper.App.Interfaces
{
    public interface IAppSettingsContext
    {
        List<AppSetting> AppSettings { get; set; }
    }
}
