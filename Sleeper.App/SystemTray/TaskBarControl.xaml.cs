using Sleeper.App.Controls;
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

namespace Sleeper.App.SystemTray
{
    /// <summary>
    /// Interaction logic for TaskBarControl.xaml
    /// </summary>
    public partial class TaskBarControl : UserControl
    {
        public TaskBarControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void ExitApplication(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
