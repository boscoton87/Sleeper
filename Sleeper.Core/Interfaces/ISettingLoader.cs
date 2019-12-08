using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleeper.Core.Interfaces
{
    public interface ISettingLoader
    {
        void ApplySetting(string setting, string value);

        string GetSetting(string setting);
    }
}
