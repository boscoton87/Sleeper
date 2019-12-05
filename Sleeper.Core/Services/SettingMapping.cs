using System;

namespace Sleeper.Core.Services
{
    public class SettingMapping
    {
        public Func<string> Load { get; set; }
        public Action<string> Apply { get; set; }
    }
}
