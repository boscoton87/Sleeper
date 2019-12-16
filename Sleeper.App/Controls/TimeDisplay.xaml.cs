using Sleeper.Core.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sleeper.App.Controls
{
    /// <summary>
    /// Interaction logic for TimeDisplay.xaml
    /// </summary>
    public partial class TimeDisplay : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int? NumberValue { get; set; } = 0;

        public int UnitsToMinutes { get; set; }

        public string Units { get; set; }

        public Action<int> ApplyDelay { get; set; }

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
            if(NumberValue >= 99)
            {
                return;
            }
            var offset = IncrementDelayCount(UnitsToMinutes, NumberValue * UnitsToMinutes) / UnitsToMinutes;
            ApplyOffset(offset);
        }

        private void DecrementDisplayValue(object sender, MouseEventArgs e)
        {
            if (NumberValue <= 0)
            {
                return;
            }
            var offset = IncrementDelayCount((-1 * UnitsToMinutes), NumberValue * UnitsToMinutes) / UnitsToMinutes;
            ApplyOffset(offset);
        }

        private void ApplyOffset(int offset)
        {
            if (!NumberValue.HasValue)
            {
                NumberValue = offset < 0 ? 0 : offset;
            }
            else
            {
                NumberValue += offset;
            }
            OnPropertyChanged("DisplayTextBox");
        }

        private int IncrementDelayCount(int delayInMinutes, int? referenceValue)
        {
            var offset = (referenceValue + delayInMinutes) < 0 ? 0 : delayInMinutes;
            ApplyDelay(offset);
            return offset;
        }
    }
}
