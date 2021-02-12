using System;
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
            DateTime startParse = (DateTime)StartDate.SelectedDate;
            DateTime endParse = (DateTime)EndDate.SelectedDate;
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

        private void StartDateChanged(object sender, RoutedEventArgs e)
        {
            if (StartDate.SelectedDate == null)
            {
                StartDate.SelectedDate = DateTime.Today;
            }
            if (EndDate.SelectedDate <= StartDate.SelectedDate)
            {
                EndDate.SelectedDate = ((DateTime)StartDate.SelectedDate).AddDays(1);
            }
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, (DateTime)StartDate.SelectedDate);
            EndDate.BlackoutDates.Add(blackoutRange);
        }
        private void EndDateChanged(object sender, RoutedEventArgs e)
        {
            if (EndDate.SelectedDate == null)
            {
                EndDate.SelectedDate = ((DateTime)StartDate.SelectedDate).AddDays(1);
            }
        }
        private void SetDatepickersSettings()
        {
            StartDate.SelectedDate = DateTime.Today;
            EndDate.SelectedDate = DateTime.Today.AddDays(1);

            StartDate.BlackoutDates.AddDatesInPast();
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, (DateTime)StartDate.SelectedDate);
            EndDate.BlackoutDates.Add(blackoutRange);
        }
        private void DontClick_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
                System.Diagnostics.Process.Start("https://youtu.be/UH9f6nqA0Gk");
            for (int i = 0; i < 1; i++)
                MessageBox.Show("ау ты шо там делаешь");
        }
    }
}
