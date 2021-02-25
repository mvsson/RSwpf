using System;
using System.Windows;
using System.Windows.Controls;

namespace RSwpf.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetDatepickersBlackout();
        }
        private void StartDateChanged(object sender, RoutedEventArgs e)
        {
            if (StartDate.SelectedDate == null)
                StartDate.SelectedDate = DateTime.Today;
            if (EndDate.SelectedDate == null)
                EndDate.SelectedDate = DateTime.Today.AddDays(1);
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, StartDate.SelectedDate.Value);
            EndDate.BlackoutDates.Clear();
            EndDate.BlackoutDates.Add(blackoutRange);
        }
        private void SetDatepickersBlackout()
        {
            StartDate.BlackoutDates.AddDatesInPast();
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, DateTime.Today);
            EndDate.BlackoutDates.Add(blackoutRange);
        }
        private void OpenSettingsClick(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }
        private void ResetZoomOnClick1(object sender, RoutedEventArgs e)
        {
            X.MinValue = double.NaN;
            X.MaxValue = double.NaN;
        }
        private void ResetZoomOnClick2(object sender, RoutedEventArgs e)
        {
            X2.MinValue = double.NaN;
            X2.MaxValue = double.NaN;
        }
        private void ResetZoomOnClick3(object sender, RoutedEventArgs e)
        {
            X3.MinValue = double.NaN;
            X3.MaxValue = double.NaN;
        }
    }
}
