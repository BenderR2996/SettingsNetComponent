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
        public AppIdentifier AppId { get; private set; }
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
        public NetTcpJsonProvider(AppIdentifier appId, Uri address)
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
        public NetTcpJsonProvider(AppIdentifier appId, IPAddress serverIP, uint serverPort, string serviceName)
            : this(appId, new Uri($"net.tcp://{serverIP}:{serverPort}/{serviceName}")) { }

        /// <summary>
        /// Сопоставляет Mac-адрес одного из сетевых интерфейсов на клиентской машине именно с тем Mac-адресом, 
        /// с которым ассоциирован Ip-адрес вызова на серверное приложение
        /// </summary>
        /// <remarks>
        /// Этот метод нужен для хостов имеющих более одного сетевого интерфейса.
        /// </remarks>
        /// <param name="sc">Ссылка на запущенный сервис</param>
        /// <returns>
        /// Возвращает Mac-адресс сетевого интерфейса. Если подключение к сети отсустсвует, возвратит первый из списка.
        /// </returns>
        private string GetMyMac(ServiceClient sc)
        {
            string GetDefaultMac() => (new SystemInfo()).GetIPMacPairs().First().MacAddress;
            try
            {
                var ip = sc.GetIpAddress();
                var mac = (new SystemInfo()).GetMacByIp(ip);
                if (!string.IsNullOrEmpty(mac))
                    return mac;
            }
            catch
            {
                return GetDefaultMac();
            }
            return GetDefaultMac();
        }

        /// <summary>
        /// Ассоциирует приложение и Mac-адрес (однократно)
        /// </summary>
        /// <remarks>нужно для однозначной идентификации одинаковых приложений на разных узлах одной сети</remarks>
        /// <param name="sc"></param>
        private void AssociateToMac(ServiceClient sc)
        {
            if (string.IsNullOrEmpty(AppId.Mac))
            {
                var mac = GetMyMac(sc);
                if (!mac.Equals("Not connected!"))
                {
                    AppId.Mac = mac;
                }
            }
        }

        public string LoadJson()
        {
            try
            {
                using (var sc = new ServiceClient(Address))
                {
                    if (sc.Connect())
                    {
                        AssociateToMac(sc);
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
                        AssociateToMac(sc);
                        AppSettingsRecord record = new AppSettingsRecord() { AppId = this.AppId, JsonData = json };
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
