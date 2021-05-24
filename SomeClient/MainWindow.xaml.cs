using SettingsNetComponent;
using System;
using System.Collections.Generic;
using System.IO;
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
        private const string GuidFile = "guid";
        public static Guid LoadGuid()
        {
            if (!File.Exists(GuidFile))
            {
                File.WriteAllText(GuidFile, Guid.NewGuid().ToString());
            }
            return Guid.Parse(File.ReadAllText(GuidFile));
        }

        public LocalAppSettings AppSettings { get; set; } = new LocalAppSettings();
        ISettingsProvider<LocalAppSettings> SettingsProvider { get; set; }



        public MainWindow()
        {
            InitializeComponent();
            AppSettings = new LocalAppSettings();
            this.DataContext = AppSettings;
            this.Title = Assembly.GetExecutingAssembly().GetName().Name;

            SettingsProvider = new SettingsProvider<LocalAppSettings>(
                new NetTcpJsonProvider(
                        LoadGuid(),
                        new Uri($@"net.tcp://192.168.221.198:15001/SettingsService")
                    )
                );

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
