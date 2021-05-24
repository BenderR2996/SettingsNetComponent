using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SettingsNetComponent
{
    /// <summary>
    /// Реализация сервисного контракта INetContract
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public sealed class NetService : INetContract
    {
        public bool CheckConnection()
        {
            return true;
        }

        /// <summary>
        /// Вернуть ip-адрес соединения
        /// </summary>
        /// <returns>Ip-адрес</returns>
        public string GetIpAddress()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

            if (endpoint != default(RemoteEndpointMessageProperty))
            {
                return endpoint.Address;
            }
            return "";
        }

        /// <summary>
        /// Загрузка данных по идентификатору (приложение/хост)
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public AppSettingsRecord LoadSettings(Guid appId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Сохранение настроек
        /// </summary>
        /// <param name="appSettings">Строка Json-настроек ассоциированная с идентификатором приложения</param>
        public void SaveSettings(AppSettingsRecord appSettings)
        {
            throw new NotImplementedException();
        }
    }
}
