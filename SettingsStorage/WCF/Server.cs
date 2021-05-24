using SettingsNetComponent;
using System;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace SettingsStorage.WCF
{
    /// <summary>
    /// 
    /// </summary>
    public class Server : DependencyObject
    {
        #region Address & Binding
        Uri address;
        NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
        ServiceHost service;

        /// <summary>
        /// Ip-адрес сервера
        /// </summary>
        public IPAddress ServerIP { get; set; } = IPAddress.Parse("127.0.0.1");
        /// <summary>
        /// Порт
        /// </summary>
        public uint ServerPort => 15001;
        /// <summary>
        /// Название службы
        /// </summary>
        public string ServiceName => "SettingsService";
        #endregion

        public event Action<CommunicationException> ServerCatchError;

        #region SERVER STATE
        private CommunicationState state; // State server

        public event Action<CommunicationState> StateChanged;
        public CommunicationState State
        {
            get => state; private set
            {
                state = value;
                StateChanged?.Invoke(state);
            }
        }
        #endregion

        /// <summary>
        /// Инициализация сервиса
        /// </summary>
        private void InitService()
        {
            address = new Uri($"net.tcp://{ServerIP}:{ServerPort}/{ServiceName}");
            service = new ServiceHost(typeof(NetService), address);

            if (State != CommunicationState.Opened && State != CommunicationState.Opening)
            {
                service.Opening += (s, e) => { State = CommunicationState.Opening; };
                service.Closing += (s, e) => { State = CommunicationState.Closing; };
                service.Closed += (s, e) => { State = CommunicationState.Closed; };
                service.Opened += (s, e) => { State = CommunicationState.Opened; };
                service.Faulted += (s, e) => { State = CommunicationState.Opened; };

                try
                {
                    service.AddServiceEndpoint(typeof(INetContract), binding, address.AbsoluteUri);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

        /// <summary>
        /// Запуск сервера
        /// </summary>
        public async void Start()
        {
            if (State != CommunicationState.Opened)
                await Task.Run(() =>
                {
                    try
                    {
                        if (service == null ||
                            service?.State != CommunicationState.Opened ||
                        service?.State != CommunicationState.Opening)
                        {
                            InitService();
                        }
                    }
                    catch (Exception ex)
                    {
                        service = null;
                        State = CommunicationState.Faulted;
                        ServerCatchError?.Invoke(new CommunicationException(ex.Message, ex));
                    }
                    if (service != null && service.State != CommunicationState.Opened && service.State != CommunicationState.Opening)
                    {
                        try
                        {
                            service?.Open();
                        }
                        catch (Exception err)
                        {
                            State = CommunicationState.Faulted;
                            ServerCatchError(new CommunicationException(err.Message, err));
                        }

                    }
                });
        }
        /// <summary>
        /// Остановка сервера
        /// </summary>
        public async void Stop()
        {
            if (State == CommunicationState.Opened)
                await Task.Run(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        try
                        {
                            service.Abort();
                            service = null;
                        }
                        catch (Exception err)
                        {
                            ServerCatchError?.Invoke(new CommunicationException(err.Message, err));
                        }
                    });
                });
        }
    }
}
