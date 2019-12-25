using Sleeper.Taskbar.Controls;
using Sleeper.Taskbar.Interfaces;

namespace Sleeper.Taskbar.Models
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
