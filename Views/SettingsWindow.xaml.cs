using System.Windows;

namespace RateShopperWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }
        private void SaveSettingsClick(object sender, RoutedEventArgs e)
        {
            App.IOService.SaveData(App.UserSettings);
            Close();
        }
    }
}
