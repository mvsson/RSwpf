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
        private void DeclineClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
