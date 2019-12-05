using System;
using System.Collections.Generic;

namespace Sleeper.App.Interfaces
{
    public interface IAppSettingsContext
    {
        List<string> ChildControlNames { get; set; }

        Dictionary<string, List<Action<string>>> ChildChangeEmitters { get; set; }

        string HibernateEnabledTextBox { get; set; }

        string IsAdminTextBox { get; set; }
    }
}
