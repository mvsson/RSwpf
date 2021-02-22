using System;
using System.Windows;
using RateShopperWPF.Services.FileIO;
using RateShopperWPF.ViewModels.UserSettings;
using RateShopperWPF.Views;

namespace RateShopperWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly UserSettings UserSettings;
        internal static readonly FileIOService IOService;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainWindow() { DataContext = UserSettings.MainVM }.Show();
        }

        static App ()
        {
            IOService = new FileIOService($"{Environment.CurrentDirectory}\\Settings.json");
            UserSettings = IOService.LoadSettings();
            if (UserSettings == null)
            {
                UserSettings = new UserSettings();
                IOService.SaveData(UserSettings);
            }
        }
    }
}
