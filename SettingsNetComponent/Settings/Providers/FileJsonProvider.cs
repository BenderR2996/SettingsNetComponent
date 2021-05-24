using System;
using System.IO;

namespace SettingsNetComponent
{
    /// <summary>
    /// Поставщик настроек в Json формате (из локального файла settings.json)
    /// Нужен для отладочных целей
    /// </summary>
    public class FileJsonProvider : IJsonProvider
    {
        private readonly string path = "settings.json";       
        public Guid AppId { get; set; }
        public string LoadJson()
        {
            return File.ReadAllText(path);
        }
        public void SaveJson(string json)
        {
            File.WriteAllText(path, json);
        }
    }
}
