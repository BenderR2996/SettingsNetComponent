namespace SettingsNetComponent
{
    /// <summary>
    /// Провайдер настроек
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISettingsProvider<T> where T : class//, new()
    {
        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <returns></returns>
        T Load();
        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="settingsClass"></param>
        void Save(T settingsClass);
    }
}