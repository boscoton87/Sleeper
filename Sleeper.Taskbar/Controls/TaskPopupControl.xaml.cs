using Sleeper.Core.Interfaces;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sleeper.Taskbar.Controls
{
    /// <summary>
    /// Interaction logic for TaskPopupControl.xaml
    /// </summary>
    public partial class TaskPopupControl : UserControl, INotifyPropertyChanged
    {
        private const string StartProcessText = "Set";
        private const string CancelProcessText = "Cancel";
        private const int MaximumDurationInMinutes = 5999;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ExecuteButtonText { get; set; } = StartProcessText;

        private void TriggerDelayedAction(object sender, MouseEventArgs e)
        {
            if (ExecuteButtonText == StartProcessText)
            {
                var minuteDisplay = ((TimeDisplay)FindName("MinutesDisplay"));
                var hourDisplay = ((TimeDisplay)FindName("HoursDisplay"));
                var delayInMinutes = minuteDisplay.NumberValue.HasValue ? minuteDisplay.NumberValue.Value : 0;
                delayInMinutes += hourDisplay.NumberValue.HasValue ? (hourDisplay.NumberValue.Value * 60) : 0;
                delayInMinutes = delayInMinutes > MaximumDurationInMinutes ? MaximumDurationInMinutes : delayInMinutes;
                Facade.Services.Container.ResolveGlobalInstance<IDelayedActionService>().ExecuteActionOnDelay(delayInMinutes);
                ExecuteButtonText = CancelProcessText;
            }
            else
            {
                Facade.Services.Container.ResolveGlobalInstance<IDelayedActionService>().CancelDelayedAction();
                ExecuteButtonText = StartProcessText;
            }
            OnPropertyChanged("ExecuteButtonText");
        }

        public TaskPopupControl()
        {
            InitializeComponent();
            DataContext = this;
            Facade.Services.Container.ResolveGlobalInstance<IDelayedActionService>().RegisterDelayChangeEmitter(newDelay =>
            {
                var minuteDisplayControl = ((TimeDisplay)FindName("MinutesDisplay"));
                var hourDisplayControl = ((TimeDisplay)FindName("HoursDisplay"));
                hourDisplayControl.DisplayTextBox = (newDelay / 60).ToString();
                minuteDisplayControl.DisplayTextBox = (newDelay % 60).ToString();
                if (newDelay == 0)
                {
                    ExecuteButtonText = StartProcessText;
                    OnPropertyChanged("ExecuteButtonText");
                }
            });
            var minuteDisplay = ((TimeDisplay)FindName("MinutesDisplay"));
            minuteDisplay.UnitsToMinutes = 1;
            minuteDisplay.IncrementValue = 15;
            minuteDisplay.Units = "m";
            minuteDisplay.ApplyDelay = ApplyDelayOffset;
            minuteDisplay.TryCarryToNextPosition = (value) =>
            {
                if (HoursDisplay.NumberValue + value > HoursDisplay.MaximumValue)
                {
                    return false;
                }
                HoursDisplay.ApplyOffset(value);
                return true;
            };
            minuteDisplay.TakeFromNextPosition = (requestedValue) =>
            {
                if (HoursDisplay.NumberValue < requestedValue)
                {
                    return null;
                }
                HoursDisplay.ApplyOffset(-requestedValue);
                return requestedValue;
            };
            var hourDisplay = ((TimeDisplay)FindName("HoursDisplay"));
            hourDisplay.UnitsToMinutes = 60;
            hourDisplay.Units = "h";
            hourDisplay.ApplyDelay = ApplyDelayOffset;
            minuteDisplay.ButtonVisibility = Visibility.Visible;
            hourDisplay.ButtonVisibility = Visibility.Collapsed;
        }

        private void ApplyDelayOffset(int offset)
        {
            if (ExecuteButtonText == CancelProcessText)
            {
                Facade.Services.Container.ResolveGlobalInstance<IDelayedActionService>().IncreaseDelay(offset);
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
