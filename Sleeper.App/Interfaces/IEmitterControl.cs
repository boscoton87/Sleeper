using Sleeper.App.Models;

namespace Sleeper.App.Interfaces
{
    public interface IEmitterControl
    {
        AppSetting AppSetting { get; set; }

        void SetState(string state);

        void SetEnabled(bool isEnabled);
    }
}
