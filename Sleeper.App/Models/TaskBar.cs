using Hardcodet.Wpf.TaskbarNotification;
using Sleeper.App.Interfaces;
using Sleeper.App.SystemTray;

namespace Sleeper.App.Models
{
    public class TaskBar : ITaskBar
    {
        public TaskBarControl TaskBarIcon { get; }

        public object DataContext { get; }
        public TaskBar(TaskBarControl taskBarIcon, object dataContext)
        {
            TaskBarIcon = taskBarIcon;
            DataContext = dataContext;
        }
    }
}
