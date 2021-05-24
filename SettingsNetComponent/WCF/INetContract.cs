using System;
using System.ServiceModel;

namespace SettingsNetComponent
{
    /// <summary>
    /// Сервисный контракт для сетевого взаимодействия клиентских машин и сервера
    /// для централизованного хранения настроек различных приложений на этих машинах.
    /// </summary>
    [ServiceContract]
    public interface INetContract
    {
        [OperationContract]
        bool CheckConnection();
        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="appSettings"></param>
        [OperationContract]
        void SaveSettings(AppSettingsRecord appSettings);
        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <param name="appId">Идентификатор (приложение/хост)</param>
        /// <returns>Возвращает экземпляр объекта AppSettingsRecord</returns>
        [OperationContract(IsOneWay = false)]
        AppSettingsRecord LoadSettings(Guid appId);
        /// <summary>
        /// Возвращает Ip-адрес клиента, с которым было установлено соединение.
        /// </summary>
        /// <remarks>
        /// Нужен для однозначного определения сетевого интерфейса на клиентской машине с целью сопоставления с приложением, сохраняющим настройки в БД.
        /// </remarks>
        /// <returns>Ip-адрес</returns>
        [OperationContract(IsOneWay = false)]
        string GetIpAddress();
    }
}
