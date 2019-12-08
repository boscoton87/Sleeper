using Sleeper.Core.Services;
using System;
using System.Collections.Generic;

namespace Sleeper.Core.Interfaces
{
    public interface ISettingManager
    {
        void RegisterSettingChangeEmitter(Action<Dictionary<string, string>> settingChangeEmitter);

        void RegisterSettingMapping(string setting, SettingMapping mapper);

        void UpdateSetting(string setting, string value);

        void ApplySettings();

        Dictionary<string, string> GatherSettings();

        Dictionary<string, string> GetSettings();
    }
}