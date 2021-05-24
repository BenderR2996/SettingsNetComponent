using SettingsStorage.WCF;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;

namespace SettingsStorage.ViewModel
{
    public class ServerViewModel : DependencyObject
    {
        public ServerViewModel()
        {
            InitializationServer();
            RefreshIpList.Execute(null);
            SelectedIp = IPAddressList?.First();
        }
        protected Server Server { get; private set; }
        private void InitializationServer()
        {
            Server = new Server();
            Server.StateChanged += (st) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    BtnStartStopText = (Server.State == System.ServiceModel.CommunicationState.Opened) ? "Stop" : "Start";
                    IsServerStarted = (Server.State == System.ServiceModel.CommunicationState.Opened) ? true : false;
                }));
            };
        }

        public bool IsServerStarted
        {
            get { return (bool)GetValue(IsServerStartedProperty); }
            set { SetValue(IsServerStartedProperty, value); }
        }
        public static readonly DependencyProperty IsServerStartedProperty =
            DependencyProperty.Register("IsServerStarted", typeof(bool), typeof(ServerViewModel), new PropertyMetadata(false));

        #region Ip address        
        public IPAddress SelectedIp
        {
            get { return (IPAddress)GetValue(SelectedIpProperty); }
            set { SetValue(SelectedIpProperty, value); }
        }
        public static readonly DependencyProperty SelectedIpProperty = DependencyProperty.Register("SelectedIp", typeof(IPAddress), typeof(ServerViewModel));

        public ObservableCollection<IPAddress> IPAddressList
        {
            get { return (ObservableCollection<IPAddress>)GetValue(IPAddressListProperty); }
            set { SetValue(IPAddressListProperty, value); }
        }

        public static readonly DependencyProperty IPAddressListProperty =
            DependencyProperty.Register("IPAddressList", typeof(ObservableCollection<IPAddress>), typeof(ServerViewModel));

        #region Обновить список Ip адресов
        private RelayCommand refreshIpList;
        public RelayCommand RefreshIpList => refreshIpList ?? (refreshIpList = new RelayCommand(
        (parametr) =>
        {
            IPAddressList = new ObservableCollection<IPAddress>(Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
        },
        (o) => { return Server.State != System.ServiceModel.CommunicationState.Opened; }
        ));
        #endregion

        #endregion

        #region Запуск/Остановка Сервиса

        public string BtnStartStopText
        {
            get { return (string)GetValue(BtnStartStopTextProperty); }
            set { SetValue(BtnStartStopTextProperty, value); }
        }

        public static readonly DependencyProperty BtnStartStopTextProperty =
            DependencyProperty.Register("BtnStartStopText", typeof(string), typeof(ServerViewModel), new PropertyMetadata("Start"));

        private RelayCommand startService;
        public RelayCommand StartStopService => startService ?? (startService = new RelayCommand(
        (parametr) =>
        {
            // Старт сервиса
            Server.ServerIP = SelectedIp;

            if (Server.State != System.ServiceModel.CommunicationState.Opened)
            {
                Server.Start();
            }
            else
            {
                Server.Stop();
            }
        }
        ));
        #endregion
    }
}
