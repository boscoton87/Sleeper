using Sleeper.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sleeper.Core.Services
{
    public class SettingManager : ISettingManager
    {
        private Dictionary<string, SettingMapping> SettingMappings { get; } = new Dictionary<string, SettingMapping>();

        private Dictionary<string, string> CachedSettings { get; } = new Dictionary<string, string>();

        private List<Action<Dictionary<string, string>>> SettingChangeEmitters { get; } = new List<Action<Dictionary<string, string>>>();

        public void RegisterSettingMapping(string setting, SettingMapping mapping)
        {
            if (SettingMappings.ContainsKey(setting))
            {
                SettingMappings[setting] = mapping;
            } else
            {
                SettingMappings.Add(setting, mapping);
            }
        }

        public void RegisterSettingChangeEmitter(Action<Dictionary<string, string>> settingChangeEmitter)
        {
            SettingChangeEmitters.Add(settingChangeEmitter);
        }

        public void UpdateSetting(string setting, string value)
        {
            if (!CachedSettings.ContainsKey(setting))
            {
                CachedSettings.Add(setting, value);
            } else
            {
                CachedSettings[setting] = value;
            }
        }

        public void ApplySettings()
        {
            foreach(var settingMapper in SettingMappings)
            {
                settingMapper.Value.Apply(CachedSettings[settingMapper.Key]);
            }
            GatherSettings();
            SettingChangeEmitters.ForEach(emitter => emitter(CachedSettings));
        }

        public Dictionary<string, string> GatherSettings()
        {
            CachedSettings.Clear();
            foreach(var settingMapper in SettingMappings)
            {
                CachedSettings.Add(settingMapper.Key, settingMapper.Value.Load());
            }
            SettingChangeEmitters.ForEach(emitter => emitter(CachedSettings));
            return CachedSettings.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        public Dictionary<string, string> GetSettings()
        {
            return CachedSettings.ToDictionary(entry => entry.Key, entry => entry.Value);
        }
    }
}
