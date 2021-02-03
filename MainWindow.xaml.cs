using System;
using System.Collections.Generic;
using System.Linq;
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

            // Создаём список для парсинга
            UrlSettings hotelUrlSettings = new UrlSettings(hotelLink);
            DateSettings parsingDates = new DateSettings(startParse, endParse);
            string[] urls = hotelUrlSettings.getUrlsList(parsingDates);
            Progress.Maximum = urls.Length;

            //PriceParser.GetPricesList(urls, ref Progress, showDetailed.IsChecked.Value, hotelUrlSettings, parsingDates, ref outputBoard);
            
            // Получаем вывод
            var pricesList = new List<List<List<string>>>();
            DateTime dateCount = parsingDates.Start;
            if (!showDetailed.IsChecked.Value)
                outputBoard.Text += "Минимальные цены в отеле " + hotelUrlSettings.HotelLink + ", на даты:" + "\n";
            for (int i = 0; i < urls.Length; i++)  // перебираем даты
            {
                pricesList.Add(await PriceParser.GetDayPricesAsync(urls[i]));
                try
                {
                    if (showDetailed.IsChecked.Value) // "показать подробно"
                    {
                        outputBoard.Text += "\n\tЦена на дату: " + dateCount.ToString("yyyy-MM-dd") + $" В отеле [{hotelUrlSettings.HotelLink}]" + "\n";
                        for (int ii = 0; ii < pricesList[i].Count; ii++) // перебираем все цены на дату
                            if (pricesList[i][ii].Count > 1) // если больше 1 значения, значит в блоке под индексом 0 - название, 1 - цена
                                outputBoard.Text += pricesList[i][ii][1] + "\t" + pricesList[i][ii][0] + "\n";
                    }
                    else
                    {
                        outputBoard.Text += dateCount.ToString("yyyy-MM-dd") + "\t";
                        outputBoard.Text += pricesList[i][0][1] + "\t" + pricesList[i][0][0] + "\n";
                    }
                }
                catch
                {
                    outputBoard.Text += "Возникла ошибка, проверьте наличие цен на выбранные даты\n";
                }
                dateCount = dateCount.AddDays(parsingDates.Step);
                Progress.Value = i + 1;
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
