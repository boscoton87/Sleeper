using Sleeper.Taskbar.Controls;

namespace Sleeper.Taskbar.Interfaces
{
    public interface ITaskBar
    {
        TaskBarControl TaskBarIcon { get; }

        object DataContext { get; }
    }
}
