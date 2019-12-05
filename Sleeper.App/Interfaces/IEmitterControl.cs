using System;
using System.Collections.Generic;

namespace Sleeper.App.Interfaces
{
    public interface IEmitterControl
    {
        List<Action<string>> ChangeEmitters { get; set; }

        void SetState(string state);
    }
}
