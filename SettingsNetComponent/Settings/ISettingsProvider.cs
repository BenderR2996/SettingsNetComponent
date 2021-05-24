namespace SettingsNetComponent
{
    /// <summary>
    /// Провайдер настроек
    /// </summary>
    /// <typeparam name="S"></typeparam>
    public interface ISettingsProvider<S> where S : class//, new()
    {
        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <returns></returns>
        S Load();
        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="settingsClass"></param>
        void Save(S settingsClass);
    }
}