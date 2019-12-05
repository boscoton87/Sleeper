using Sleeper.App.Controls;
using Sleeper.App.Models;
using System;
using System.Collections.Generic;

namespace Sleeper.App.Interfaces
{
    public interface IEmitterControl
    {
        AppSetting AppSetting { get; set; }

        void SetState(string state);

        void SetEnabled(bool isEnabled);
    }
}
