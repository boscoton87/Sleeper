﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleeper.App.Models
{
    public class AppSetting
    {
        public string ControlName { get; set; }

        public string SettingName { get; set; }

        public bool IsEnabled { get; set; }

        public string LockedMessage { get; set; }

        public string Value { get; set; }
    }
}
