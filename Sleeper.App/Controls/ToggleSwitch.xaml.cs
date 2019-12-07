using Facade.Services;
using Sleeper.App.Interfaces;
using Sleeper.App.Models;
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
        public AppSetting AppSetting { get; set; }

        public ToggleSwitch()
        {
            InitializeComponent();
        }

        public void SetState(string state)
        {
            var stateValue = bool.Parse(state);
            GetChildGrid().HorizontalAlignment = stateValue ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            AppSetting.Value = state;
        }

        public void SetEnabled(bool isEnabled)
        {
            var parentGrid = (Grid)Content;
            parentGrid.IsEnabled = isEnabled;
        }

        private void ToggleState(object sender, MouseButtonEventArgs e)
        {
            Grid childGrid = GetChildGrid();
            childGrid.HorizontalAlignment = childGrid.HorizontalAlignment == HorizontalAlignment.Left ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            AppSetting.Value = (childGrid.HorizontalAlignment == HorizontalAlignment.Right).ToString();
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
