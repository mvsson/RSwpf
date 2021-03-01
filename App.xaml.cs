using System;
using System.Windows;
using RSwpf.Services.FileIOService;
using RSwpf.Services.PopUpMessageService;
using RSwpf.ViewModels;
using RSwpf.Views;

namespace RSwpf
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly UserSettingsViewModel UserSettings;
        internal static readonly FileIOService<UserSettingsViewModel> IOService;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainWindow() 
            { 
                DataContext = new MainWindowViewModel (new PopUpMessageBoxSender().ShowMessage) 
            }.Show();
        }

        static App ()
        {
            IOService = new FileIOService<UserSettingsViewModel>($"{Environment.CurrentDirectory}\\Settings.json");
            UserSettings = IOService.LoadSettings();
            if (UserSettings == null)
            {
                UserSettings = new UserSettingsViewModel();
                IOService.SaveData(UserSettings);
            }
        }
    }
}
