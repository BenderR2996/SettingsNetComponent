using System;
using System.ServiceModel;

namespace SettingsNetComponent
{
    /// <summary>
    /// Клиент сервиса централизованного хранения настроек
    /// </summary>
    public partial class ServiceClient : IDisposable
    {
        private ChannelFactory<INetContract> factory = null;
        private INetContract channel = null;
        private EndpointAddress serverEndpoint = null;

        /// <summary>
        /// Возвращает экземпляр клиента
        /// </summary>
        /// <param name="address">Адрес сервера хранения настроек</param>
        public ServiceClient(Uri address)
        {
            serverEndpoint = new EndpointAddress(address);
        }


        /// <summary>
        /// Сохранить настройки
        /// </summary>
        /// <param name="appSettings"></param>
        public void SaveSettings(AppSettingsRecord appSettings)
        {
            channel?.SaveSettings(appSettings);
        }
        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public AppSettingsRecord LoadSettings(AppIdentifier appId)
        {
            return channel?.LoadSettings(appId) ?? null;
        }
        /// <summary>
        /// Получить Ip-адрес соединения
        /// </summary>
        /// <returns></returns>
        public string GetIpAddress()
        {
            return channel?.GetIpAddress() ?? "";
        }

        public bool CheckConnection()
        {
            return channel?.CheckConnection() ?? false;
        }


        /// <summary>
        /// Соединиться с сервисом
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            if (serverEndpoint == null) return false;
            channel = null;
            if (factory == null)
            {
                factory = new ChannelFactory<INetContract>(
                    TcpBindingFactory.Create(),
                    serverEndpoint
                );
            }
            if (channel == null)
            {
                try
                {
                    channel = factory.CreateChannel(serverEndpoint);
                    if (factory != null && channel != null)
                    {
                        if (factory.State == CommunicationState.Opened)
                        {
                            //var ind = channel?.CheckConnection();
                            //System.Diagnostics.Debug.WriteLine(channel.GetIpAddress());
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    factory = null;
                    channel = null;
                    GC.Collect();
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// Отсоединиться от сервиса
        /// </summary>
        /// <returns></returns>
        public void Disconnect()
        {
            if (factory?.State == CommunicationState.Opened)
            {
                try
                {
                    this.factory?.Close();
                    this.channel = null;
                    this.factory = null;
                }
                catch (CommunicationException ce)
                {
                    factory?.Abort();
                    this.channel = null;
                    this.factory = null;
                }
            }
        }

        public void Dispose()// Освободить ресурсы
        {
            try
            {
                Disconnect();
            }
            catch (Exception e)
            {
                throw new Exception(innerException: e, message: "Не удалось корректно разорвать соединение.");
            }
        }
    }
}
