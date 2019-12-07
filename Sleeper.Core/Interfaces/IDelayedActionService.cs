using System;
using System.Threading.Tasks;

namespace Sleeper.Core.Interfaces
{
    public interface IDelayedActionService
    {
        bool ActionIsActive { get; }

        int DelayInMinutes { get; }

        void RegisterDelayChangeEmitter(Action<int> delayChangeEmitter);

        void RegisterDelayCancelEmitter(Action delayCancelEmitter);

        void IncreaseDelay(int delayInMinutes);

        void CancelDelayedAction();

        Task ExecuteActionOnDelay(int delayInMinutes);
    }
}
