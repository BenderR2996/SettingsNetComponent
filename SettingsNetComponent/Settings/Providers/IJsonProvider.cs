using System;

namespace SettingsNetComponent
{
    /// <summary>
    /// Поставщик настроек в формате Json
    /// </summary>
    public interface IJsonProvider
    {
        /// <summary>
        /// Идентификатор приложения на клиентской машине
        /// </summary>
        Guid AppId { get; }
        /// <summary>
        /// Загрузить настройки приложения в формате Json
        /// </summary>
        /// <returns></returns>
        string LoadJson();
        /// <summary>
        /// Сохранить настройки в формате Json
        /// </summary>
        /// <param name="json"></param>
        void SaveJson(string json);
    }
}
