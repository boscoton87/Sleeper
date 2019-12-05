using Facade.Services;
using Sleeper.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sleeper.App.Controls
{
    /// <summary>
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl, IEmitterControl
    {
        public List<Action<string>> ChangeEmitters { get; set; }

        public ToggleSwitch()
        {
            InitializeComponent();
        }

        public void SetState(string state)
        {
            GetChildGrid().HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), state);
        }

        private void ToggleState(object sender, MouseButtonEventArgs e)
        {
            Grid childGrid = GetChildGrid();
            childGrid.HorizontalAlignment = childGrid.HorizontalAlignment == HorizontalAlignment.Left ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            ChangeEmitters.ForEach(emitter => emitter(childGrid.HorizontalAlignment.ToString()));
        }

        private Grid GetChildGrid()
        {
            var senderGrid = (Grid)Content;
            Grid childGrid = null;
            foreach (var child in senderGrid.Children)
            {
                if (child is Grid)
                {
                    childGrid = child as Grid;
                }
            }
            return childGrid;
        }
    }
}
