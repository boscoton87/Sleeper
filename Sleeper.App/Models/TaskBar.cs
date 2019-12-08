using Hardcodet.Wpf.TaskbarNotification;
using Sleeper.App.Interfaces;

namespace Sleeper.App.Models
{
    public class TaskBar : ITaskBar
    {
        public TaskbarIcon TaskBarIcon { get; }

        public object DataContext { get; }
        public TaskBar(TaskbarIcon taskBarIcon, object dataContext)
        {
            TaskBarIcon = taskBarIcon;
            DataContext = dataContext;
        }
    }
}
