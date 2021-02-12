using System;
using System.Collections.Generic;
using System.Linq;
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
        GridCollection GridData { get; }
        public MainWindow()
        {
            InitializeComponent();
            SetDatepickersSettings();
            HotelLinkInput.ToolTip = "Вставьте относительный URL отеля с букинга,\n" +
                                    "то что между 'booking.com/hotel/ru/' и '.html'";
            GridData = new GridCollection();
            DataGridRatesOutput.ItemsSource = GridData.Source;
        }
        private async void Starter_Click(object sender, RoutedEventArgs e)
        {
            // Выключаем UI
            StarterButton.IsEnabled = false;
            IsShowDetailed.IsEnabled = false;

            // Достаём информацию из форм
            string hotelLink = HotelLinkInput.Text;
            DateTime startParse = (DateTime)(startDate.SelectedDate);
            DateTime endParse = (DateTime)(endDate.SelectedDate);
            bool isShowDetailed = IsShowDetailed.IsChecked.Value;

            // Создаём список адресов для парсинга
            UrlSettings hotelUrlSettings = new UrlSettings(hotelLink);
            DateSettings parsingDates = new DateSettings(startParse, endParse);
            UrlOnDate[] urls = hotelUrlSettings.GetUrlsList(parsingDates);
            Progress.Maximum = urls.Length;

            // Распиливаем масив URLs на куски длиной по N для обхода проблем обрыва сервера (по дефолту на 16)
            var SplitUrls = Parser.SplitUrlsListByN(urls);

            IOutput printer = isShowDetailed ? printer = new OutputDetailed() : new OutputShort();

            if (!isShowDetailed)
                TextBoxRates.Text += "Минимальные тарифы в отеле " + hotelUrlSettings.HotelLink + ", на даты:" + "\n";
            
            foreach (var _urls in SplitUrls)
            {
                try // выгружаем инфу
                {
                    RatesByDay[] daysList = await Parser.GetRatesListAsync(Progress, _urls);

                    TextBoxRates.Text += RatesByDay.GetText(printer, daysList);

                    RatesByDay.GetGrid(printer, daysList).ToList().ForEach(item => GridData.Add(item));
                }
                catch (Exception ex)
                {
                    TextBoxRates.Text += ex.Message;
                }
            }
            if (Progress.Value != Progress.Maximum)
                TextBoxRates.Text += "Таки где-то была ошибка в выгрузке данных, будь внимателен.";            

            // включаем UI
            Progress.Value = 0;
            StarterButton.IsEnabled = true;
            IsShowDetailed.IsEnabled = true;
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
