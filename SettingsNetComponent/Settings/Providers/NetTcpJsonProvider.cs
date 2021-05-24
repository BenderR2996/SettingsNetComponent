using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using SettingsNetComponent.SystemInformation;

namespace SettingsNetComponent
{
    /// <summary>
    /// Провайдер настроек в формате Json (по сети)
    /// </summary>
    /// <remarks>
    /// Обеспечивает загрузку/сохранение в пределах одной сети
    /// </remarks>
    /// <inheritdoc/>
    public class NetTcpJsonProvider : IJsonProvider
    {
        public Guid AppId { get; private set; }
        /// <summary>
        /// Адрес сервера
        /// </summary>
        /// <remarks>
        /// Uri вида net.tcp://<ip>:<port>/<serviceName>
        /// </remarks>
        public Uri Address { get; private set; }

        /// <summary>
        /// Создает экземпляр поставщика
        /// </summary>
        /// <param name="appId">Идентификатор приложения</param>
        /// <param name="address">Uri вида net.tcp://<ip>:<port>/<serviceName></param>
        public NetTcpJsonProvider(Guid appId, Uri address)
        {
            AppId = appId;
            Address = address;
        }

        /// <summary>
        /// Создает экземпляр поставщика
        /// </summary>
        /// <param name="appId">Идентификатор приложения</param>
        /// <param name="serverIP">ip-адрес сервера</param>
        /// <param name="serverPort">порт</param>
        /// <param name="serviceName">название сервиса</param>
        public NetTcpJsonProvider(Guid appId, IPAddress serverIP, uint serverPort, string serviceName)
            : this(appId, new Uri($"net.tcp://{serverIP}:{serverPort}/{serviceName}")) { }

        public string LoadJson()
        {
            try
            {
                using (var sc = new ServiceClient(Address))
                {
                    if (sc.Connect())
                    {
                        return sc.LoadSettings(AppId).JsonData;
                    }
                    else
                    {
                        throw new Exception("Ошибка подключения");
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine($@"Ошибка загрузки настроек. {err.Message}");
                return "";
            }
        }

        public void SaveJson(string json)
        {
            try
            {
                using (var sc = new ServiceClient(Address))
                {
                    if (sc.Connect())
                    {
                        AppSettingsRecord record = new AppSettingsRecord() { Id = this.AppId, JsonData = json };
                        sc.SaveSettings(record);
                    }
                    else
                    {
                        throw new Exception("Ошибка подключения");
                    }
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine($@"Ошибка сохранения настроек. {err.Message}");
            }
        }
    }
}
