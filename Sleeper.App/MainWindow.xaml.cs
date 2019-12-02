using Facade.Services;
using Sleeper.Core.Helpers;
using Sleeper.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sleeper.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string StartProcessText = "Set Sleep Timer";
        private const string CancelProcessText = "Cancel Sleep Timer";

        public event PropertyChangedEventHandler PropertyChanged;

        private bool TimerIsActive { get; set; } = false;

        public string ExecuteButtonText { get; set; } = StartProcessText;

        private int? Minutes { get; set; }
        private int? Hours { get; set; }

        public string MinutesTextBox {
            get {
                return Minutes.ToString();
            }
            set {
                int parsedMinutes;
                if (string.IsNullOrWhiteSpace(value))
                {
                    Minutes = null;
                }
                if (int.TryParse(value, out parsedMinutes))
                {
                    Minutes = parsedMinutes;
                }
            }
        }

        public string HoursTextBox
        {
            get
            {
                return Hours.ToString();
            }
            set
            {
                int parsedHours;
                if (string.IsNullOrWhiteSpace(value))
                {
                    Hours = null;
                }
                if (int.TryParse(value, out parsedHours))
                {
                    Hours = parsedHours;
                }
            }
        }

        private void TriggerDelayedAction(object sender, MouseEventArgs e)
        {
            if (ExecuteButtonText == StartProcessText)
            {
                var delayInMinutes = Minutes.HasValue ? Minutes.Value : 0;
                delayInMinutes += Hours.HasValue ? (Hours.Value * 60) : 0;
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

        private void IncrementHours(object sender, MouseEventArgs e)
        {
            var offset = IncrementDelayCount(60, Hours * 60) / 60;
            ApplyHourOffset(offset);
        }

        private void DecrementHours(object sender, MouseEventArgs e)
        {
            var offset = IncrementDelayCount(-60, Hours * 60) / 60;
            ApplyHourOffset(offset);
        }

        private void IncrementMinutes(object sender, MouseEventArgs e)
        {
            var offset = IncrementDelayCount(1, Minutes);
            ApplyMinuteOffset(offset);
        }

        private void DecrementMinutes(object sender, MouseEventArgs e)
        {
            var offset = IncrementDelayCount(-1, Minutes);
            ApplyMinuteOffset(offset);
        }

        public MainWindow()
        {
            InitializeComponent();
            Facade.Services.Container.ResolveGlobalInstance<IDelayedActionService>().RegisterDelayChangeEmitter(newDelay =>
            {
                var oldHoursTextBox = HoursTextBox;
                var oldMinutesTextBox = MinutesTextBox;
                HoursTextBox = (newDelay / 60).ToString();
                MinutesTextBox = (newDelay % 60).ToString();
                OnPropertyChanged("HoursTextBox");
                OnPropertyChanged("MinutesTextBox");
            });
            DataContext = this;
        }

        private void ApplyHourOffset(int offset)
        {
            if (!Hours.HasValue)
            {
                Hours = offset < 0 ? 0 : offset;
            }
            else
            {
                Hours += offset;
            }
            OnPropertyChanged("HoursTextBox");
        }

        private void ApplyMinuteOffset(int offset)
        {
            if (!Minutes.HasValue)
            {
                Minutes = offset < 0 ? 0 : offset;
            }
            else
            {
                Minutes += offset;
            }
            OnPropertyChanged("MinutesTextBox");
        }

        private int IncrementDelayCount(int delayInMinutes, int? referenceValue)
        {
            var offset = (referenceValue + delayInMinutes) < 0 ? 0 : delayInMinutes;
            if (ExecuteButtonText == CancelProcessText)
            {
                Facade.Services.Container.ResolveGlobalInstance<IDelayedActionService>().IncreaseDelay(offset);
            }
            return offset;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
