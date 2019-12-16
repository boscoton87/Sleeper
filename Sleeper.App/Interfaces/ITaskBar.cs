using Hardcodet.Wpf.TaskbarNotification;
using Sleeper.App.SystemTray;

namespace Sleeper.App.Interfaces
{
    public interface ITaskBar
    {
        TaskBarControl TaskBarIcon { get; }

        object DataContext { get; }
    }
}
