using Microsoft.EntityFrameworkCore;
using SettingsNetComponent;
using SettingsStorage.Model;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SettingsStorage.WCF
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
        public AppSettingsRecord LoadSettings(AppIdentifier appId)
        {
            using (var ctx = new AppDbContext())
            {
                ctx.AppSettings.Load();
                var record = ctx.AppSettings.Find(appId.GetKey());
                if (record != null)
                    return record;
            }
            return null;
        }
        /// <summary>
        /// Сохранение настроек
        /// </summary>
        /// <param name="appSettings">Строка Json-настроек ассоциированная с идентификатором приложения</param>
        public void SaveSettings(AppSettingsRecord appSettings)
        {
            using (var ctx = new AppDbContext())
            {
                var record = ctx.AppSettings.Find(appSettings.AppId.GetKey());
                if (record != null) // если запись найдена - обновить значения
                {
                    record.JsonData = appSettings.JsonData;
                }
                else // если не найдена - добавить новую
                {
                    ctx.AppSettings.Add(appSettings);
                }
                ctx.SaveChanges();
            }
        }
    }
}
