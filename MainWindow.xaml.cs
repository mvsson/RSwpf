using System;
using System.Windows;


namespace RateShopperWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            startDate.SelectedDate = DateTime.Today;
            endDate.SelectedDate = DateTime.Today.AddDays(1);
            
        }

        private async void Starter_Click(object sender, RoutedEventArgs e)
        {
            // Выключаем UI
            Starter.IsEnabled = false;
            showDetailed.IsEnabled = false;

            // Достаём информацию из форм
            string hotelLink = hotelLinkInput.Text;
            DateTime startParse = (DateTime)(startDate.SelectedDate);
            DateTime endParse = (DateTime)(endDate.SelectedDate);

            // Создаём список адресов для парсинга
            UrlSettings hotelUrlSettings = new UrlSettings(hotelLink);
            DateSettings parsingDates = new DateSettings(startParse, endParse);
            string[] urls = hotelUrlSettings.getUrlsList(parsingDates);
            Progress.Maximum = urls.Length;
            try
            {
                await PriceParser.ShowQuickOutputPricesAsync(urls, Progress, showDetailed.IsChecked.Value,
                hotelUrlSettings, parsingDates, outputBoard);
            }
            catch (Exception ex)
            {
                outputBoard.Text += ex.Message;
            }
            
            // включаем UI
            Progress.Value = 0;
            Starter.IsEnabled = true;
            showDetailed.IsEnabled = true;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
