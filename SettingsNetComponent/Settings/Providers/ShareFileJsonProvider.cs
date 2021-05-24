using System;
using System.IO;

namespace SettingsNetComponent
{

    /// <summary>
    /// Поставщик настроек в Json формате (из файла <guid>.json хранящегося в общей папке)
    /// </summary>
    public class ShareFileJsonProvider : IJsonProvider
    {
        private readonly string path;
        public ShareFileJsonProvider(Guid appId, DirectoryInfo directory)
        {
            this.AppId = appId;
            path = directory.FullName;
        }
        public Guid AppId { get; set; }
        public string LoadJson()
        {
            return File.ReadAllText($@"{path}\{AppId}");
        }
        public void SaveJson(string json)
        {
            File.WriteAllText($@"{path}\{AppId}", json);
        }
    }
}
