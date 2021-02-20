using System.Windows;
using RateShopperWPF.ViewModels.UserSettings;

namespace RateShopperWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly GlobalSettings UserSettings;
        static App ()
        {
            UserSettings = new GlobalSettings();
        }
    }
}
