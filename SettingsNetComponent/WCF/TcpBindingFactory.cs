using System;
using System.ServiceModel;

namespace SettingsNetComponent
{
    /// <summary>
    /// Класс настроек Tcp соединения
    /// </summary>
    public class TcpBindingFactory
    {
        public static NetTcpBinding Create()
        {
            return new NetTcpBinding(SecurityMode.None)
            {
                MaxBufferPoolSize = 1024 * 1024 * 10,
                MaxBufferSize = 1024 * 1024 * 10,
                MaxReceivedMessageSize = 1024 * 1024 * 10,
                //ReceiveTimeout = TimeSpan.FromMinutes(15),
                SendTimeout = TimeSpan.FromMinutes(3),
                //TransferMode = TransferMode.Buffered
            };
        }
    }
}
