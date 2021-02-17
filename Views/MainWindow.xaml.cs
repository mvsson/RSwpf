using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace RateShopperWPF.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetDatepickersSettings();

            HotelLinkInput.ToolTip = "Вставьте относительный URL отеля с букинга,\nто что между 'booking.com/hotel/ru/' и '.html'";         
        }
        private void StartDateChanged(object sender, RoutedEventArgs e)
        {
            if (StartDate.SelectedDate == null)
            {
                StartDate.SelectedDate = DateTime.Today;
            }
            else if (EndDate == null || EndDate.SelectedDate <= StartDate.SelectedDate)
            {
                if (EndDate == null)
                    EndDate = new DatePicker();
                EndDate.SelectedDate = ((DateTime)StartDate.SelectedDate).AddDays(1);
            }
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, (DateTime)StartDate.SelectedDate);
            EndDate.BlackoutDates.Clear();
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
            EndDate.SelectedDate = DateTime.Today.AddMonths(1); 

            StartDate.BlackoutDates.AddDatesInPast();
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, (DateTime)StartDate.SelectedDate);
            EndDate.BlackoutDates.Add(blackoutRange);
        }
        private void DontClick_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 1; i++)
                System.Diagnostics.Process.Start("https://youtu.be/UH9f6nqA0Gk");
            MessageBox.Show("ау ты шо там делаешь");
        }
    }
}
