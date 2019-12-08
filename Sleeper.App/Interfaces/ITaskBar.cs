using Hardcodet.Wpf.TaskbarNotification;

namespace Sleeper.App.Interfaces
{
    public interface ITaskBar
    {
        TaskbarIcon TaskBarIcon { get; }

        object DataContext { get; }
    }
}
