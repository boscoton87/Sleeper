using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sleeper.Taskbar.Controls
{
    /// <summary>
    /// Interaction logic for TimeDisplay.xaml
    /// </summary>
    public partial class TimeDisplay : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public int? NumberValue { get; set; } = 0;

        public int IncrementValue { get; set; } = 1;

        public int MaximumValue { get; set; } = 59;

        public int UnitsToMinutes { get; set; }

        public string Units { get; set; }

        public Action<int> ApplyDelay { get; set; }

        public Func<int, bool> TryCarryToNextPosition { get; set; } = (value) => { return true; };

        public Func<int, int?> TakeFromNextPosition { get; set; } = (value) => { return value; };

        public Visibility ButtonVisibility { get; set; }

        public string DisplayTextBox
        {
            get
            {
                return NumberValue.ToString();
            }
            set
            {
                int parsedNumber;
                if (string.IsNullOrWhiteSpace(value))
                {
                    NumberValue = null;
                }
                var shortenedValue = value.Length > 2 ? value.Substring(value.Length - 3, 2) : value;
                if (int.TryParse(shortenedValue, out parsedNumber))
                {
                    NumberValue = parsedNumber;
                }
                OnPropertyChanged("DisplayTextBox");
            }
        }

        public TimeDisplay()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void IncrementDisplayValue(object sender, MouseEventArgs e)
        {
            var offset = IncrementDelayCount(UnitsToMinutes * IncrementValue) / UnitsToMinutes;
            ApplyOffset(offset);
        }

        private void DecrementDisplayValue(object sender, MouseEventArgs e)
        {
            var offset = IncrementDelayCount(-1 * UnitsToMinutes * IncrementValue) / UnitsToMinutes;
            ApplyOffset(offset);
        }

        public void ApplyOffset(int offset)
        {
            if (!NumberValue.HasValue)
            {
                NumberValue = offset < 0 ? 0 : offset;
            }
            else
            {
                NumberValue += offset;
                if (NumberValue > MaximumValue)
                {
                    var valueToCarry = NumberValue.Value / (MaximumValue + 1);
                    var remainingValue = NumberValue.Value % (MaximumValue + 1);
                    NumberValue = remainingValue;
                    if (!TryCarryToNextPosition(valueToCarry))
                    {
                        NumberValue = MaximumValue;
                    }
                }
                else if (NumberValue < 0)
                {
                    var takenValue = TakeFromNextPosition(1);
                    NumberValue = takenValue.HasValue ? NumberValue + (takenValue * (MaximumValue + 1)) : 0;
                }
            }
            OnPropertyChanged("DisplayTextBox");
        }

        private int IncrementDelayCount(int delayInMinutes)
        {
            ApplyDelay(delayInMinutes);
            return delayInMinutes;
        }
    }
}
