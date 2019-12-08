using Newtonsoft.Json;
using Sleeper.Core.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace Sleeper.Core.Services
{
    public class JsonSettingLoader : ISettingLoader
    {
        private string FilePath { get; }
        public JsonSettingLoader(string filePath)
        {
            Directory.CreateDirectory(filePath);
            FilePath = Path.Combine(filePath, "settings.json");
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(new Dictionary<string, string>()));
            }
        }

        public void ApplySetting(string setting, string value)
        {
            var settings =  JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(FilePath));
            if(!settings.ContainsKey(setting))
            {
                settings.Add(setting, value);
            } else
            {
                settings[setting] = value;
            }
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(settings));
        }

        public string GetSetting(string setting)
        {

            var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(FilePath));
            return settings.ContainsKey(setting) ? settings[setting] : null;
        }
    }
}
