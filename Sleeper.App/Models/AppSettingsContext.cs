using Sleeper.App.Controls;
using Sleeper.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleeper.App.Models
{
    public class AppSettingsContext : IAppSettingsContext
    {
        public List<AppSetting> AppSettings { get; set; } = new List<AppSetting>();
    }
}
