using Newtonsoft.Json;
using System;

namespace SettingsNetComponent
{
    /// <summary>
    /// Поставщик настроек
    /// </summary>
    /// <typeparam name="T">Класс настроек. Настройки должны храниться в виде публичных свойств.</typeparam>
    public class SettingsProvider<T> : ISettingsProvider<T> where T : class
    {
        private readonly IJsonProvider provider;
        //public Uri ServerAddress { get; private set; }

        /// <summary>
        /// Возвращает экземпляр поставщика настроек
        /// </summary>
        /// <remarks>
        /// Структура настроек определяется набором публичных свойств класса S
        /// </remarks>
        /// <param name="provider">Провайдер настроек в формате Json</param>
        public SettingsProvider(IJsonProvider provider)
        {
            this.provider = provider;
        }

        public T Load()
        {
            var obj = provider.LoadJson();
            return JsonConvert.DeserializeObject<T>(obj);
        }
        public void Save(T settingsClass)
        {
            var json = JsonConvert.SerializeObject(settingsClass);
            provider.SaveJson(json);
        }
    }
}
