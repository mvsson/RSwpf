﻿using System;
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
            else if (EndDate == null || EndDate.SelectedDate <= StartDate.SelectedDate.Value.AddDays(1))
            {
                if (EndDate == null)
                    EndDate = new DatePicker();
                EndDate.SelectedDate = StartDate.SelectedDate.Value.AddDays(1);
            }
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, StartDate.SelectedDate.Value);
            EndDate.BlackoutDates.Clear();
            EndDate.BlackoutDates.Add(blackoutRange);
        }
        private void EndDateChanged(object sender, RoutedEventArgs e)
        {
            if (EndDate.SelectedDate == null)
            {
                EndDate.SelectedDate = StartDate.SelectedDate.Value.AddDays(1);
            }
        }
        private void SetDatepickersSettings()
        {
            StartDate.SelectedDate = DateTime.Today;
            EndDate.SelectedDate = DateTime.Today.AddDays(14); 

            StartDate.BlackoutDates.AddDatesInPast();
            var blackoutRange = new CalendarDateRange(DateTime.MinValue, StartDate.SelectedDate.Value);
            EndDate.BlackoutDates.Add(blackoutRange);
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
