using SettingsNetComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SomeClient
{
    public partial class MainWindow : Window
    {
        public LocalAppSettings AppSettings { get; set; } = new LocalAppSettings();
        ISettingsProvider<LocalAppSettings> SettingsProvider { get; set; } =
            new SettingsProvider<LocalAppSettings>(new NetTcpJsonProvider(
                new AppIdentifier() { Id = Assembly.GetExecutingAssembly().GetName().Name },
                new Uri($@"net.tcp://192.168.221.198:15001/SettingsService"))
        );



        public MainWindow()
        {
            InitializeComponent();
            AppSettings = new LocalAppSettings();
            this.DataContext = AppSettings;
            this.Title = Assembly.GetExecutingAssembly().GetName().Name;
        }

        private void ButtonLoadSettings(object sender, RoutedEventArgs e)
        {
            AppSettings = SettingsProvider.Load();
            this.DataContext = AppSettings;
        }

        private void ButtonSaveSettings(object sender, RoutedEventArgs e)
        {
            SettingsProvider.Save(AppSettings);
        }
    }
}
