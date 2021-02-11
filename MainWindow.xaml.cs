using System;
using System.Windows;
using System.Windows.Controls;
using RateShopperWPF.core;

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
            SetDatepickersSettings();
        }

        private async void Starter_Click(object sender, RoutedEventArgs e)
        {
            // Выключаем UI
            StarterButton.IsEnabled = false;
            showDetailed.IsEnabled = false;

            // Достаём информацию из форм
            string hotelLink = hotelLinkInput.Text;
            DateTime startParse = (DateTime)(startDate.SelectedDate);
            DateTime endParse = (DateTime)(endDate.SelectedDate);

            // Создаём список адресов для парсинга
            UrlSettings hotelUrlSettings = new UrlSettings(hotelLink);
            DateSettings parsingDates = new DateSettings(startParse, endParse);
            UrlOnDate[] urls = hotelUrlSettings.GetUrlsList(parsingDates);
            Progress.Maximum = urls.Length;

            // Распиливаем масив URLs на куски длиной по N для обхода проблем обрыва сервера (по дефолту на 16)
            var SplitUrls = Parser.SplitUrlsListByN(urls);

            if (!showDetailed.IsChecked.Value)
                outputBoard.Text += "Минимальные тарифы в отеле " + hotelUrlSettings.HotelLink + ", на даты:" + "\n";
            foreach (var _urls in SplitUrls)
            {
                try // выгружаем нужную инфу
                {
                    var daysList = await Parser.GetRatesListAsync(Progress, _urls);
                    foreach (var day in daysList)
                    {
                        if (showDetailed.IsChecked.Value)
                            outputBoard.Text += day.GetDetailedPriceText();
                        else
                            outputBoard.Text += day.GetShortPriceText();
                    }
                }
                catch (Exception ex)
                {
                    outputBoard.Text += ex.Message;
                }
            }

            if (Progress.Value != Progress.Maximum)
            {
                outputBoard.Text += "Таки где-то была ошибка в выгрузке данных, будь внимателен.";
            }

            // включаем UI
            Progress.Value = 0;
            StarterButton.IsEnabled = true;
            showDetailed.IsEnabled = true;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void StartDateChanged(object sender, RoutedEventArgs e)
        {
            if (endDate.SelectedDate <= startDate.SelectedDate)
            {
                endDate.SelectedDate = ((DateTime)startDate.SelectedDate).AddDays(1);
            }
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, (DateTime)startDate.SelectedDate);
            endDate.BlackoutDates.Add(blackoutRange);
        }

        private void SetDatepickersSettings()
        {
            startDate.SelectedDate = DateTime.Today;
            endDate.SelectedDate = DateTime.Today.AddDays(1);

            startDate.BlackoutDates.AddDatesInPast();
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, (DateTime)startDate.SelectedDate);
            endDate.BlackoutDates.Add(blackoutRange);
        }
    }
}
