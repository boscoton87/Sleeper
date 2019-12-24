using Sleeper.Core.Helpers;
using Sleeper.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sleeper.Core.Services
{
    public class DelayedActionService : IDelayedActionService
    {
        private const int MinuteInMs = 60000;

        private Action DefinedAction { get; }

        private List<Action<int>> DelayChangeEmitters { get; } = new List<Action<int>>();

        private List<Action> DelayCancelEmitters { get; } = new List<Action>();

        public bool ActionIsActive { get; private set; } = false;

        private bool CancelAction { get; set; } = false;

        public int DelayInMinutes { get; private set; }

        public DelayedActionService(Action definedAction)
        {
            DefinedAction = definedAction;
        }

        public DelayedActionService(Action definedAction, List<Action<int>> delayChangeEmitters, List<Action> delayCancelEmitters)
        {
            DefinedAction = definedAction;
            DelayChangeEmitters = delayChangeEmitters;
            DelayCancelEmitters = delayCancelEmitters;
        }

        public void RegisterDelayChangeEmitter(Action<int> delayChangeEmitter)
        {
            DelayChangeEmitters.Add(delayChangeEmitter);
        }

        public void RegisterDelayCancelEmitter(Action delayCancelEmitter)
        {
            DelayCancelEmitters.Add(delayCancelEmitter);
        }

        public void CancelDelayedAction()
        {
            CancelAction = true;
        }

        public void IncreaseDelay(int delayInMinutes)
        {
            ReportingHelpers.LogInfo($"Delay of {DelayInMinutes} increased by {delayInMinutes} minutes.");
            if ((DelayInMinutes + delayInMinutes) < 0)
            {
                DelayInMinutes = 0;
            } else
            {
                DelayInMinutes += delayInMinutes;
            }
        }

        public async Task ExecuteActionOnDelay(int delayInMinutes)
        {
            try
            {
                if (!ActionIsActive)
                {
                    ActionIsActive = true;
                    DelayInMinutes = delayInMinutes;
                    ReportingHelpers.LogInfo($"Broadcasting Initial Change Emitter message.  Delay in minutes: {DelayInMinutes}.");
                    DelayChangeEmitters.ForEach(emitter => emitter(DelayInMinutes));
                    while (DelayInMinutes > 0)
                    {
                        var cyclesPerMinute = 1000;
                        for (int index = 0; index < cyclesPerMinute; index++)
                        {
                            await Task.Delay(MinuteInMs / cyclesPerMinute);
                            if (CancelAction)
                            {
                                CancelAction = false;
                                ActionIsActive = false;
                                ReportingHelpers.LogInfo($"Broadcasting Cancelled Emitter message.");
                                DelayCancelEmitters.ForEach(emitter => emitter());
                                return;
                            }
                        }
                        DelayInMinutes--;
                        ReportingHelpers.LogInfo($"Broadcasting Change Emitter message.  Remaining delay in minutes: {DelayInMinutes}.");
                        DelayChangeEmitters.ForEach(emitter => emitter(DelayInMinutes));
                    }
                    ReportingHelpers.LogInfo($"Executing delayed action.");
                    DefinedAction();
                    ActionIsActive = false;
                }
            }
            catch(Exception e)
            {
                ReportingHelpers.LogError($"Error executing Delayed Action: {e}");
            }
        }
    }
}
