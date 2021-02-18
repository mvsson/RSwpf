using System;
using LiveCharts;
using LiveCharts.Configurations;
using RateShopperWPF.Models;

namespace RateShopperWPF.ViewModels
{
    public partial class MainViewModel
    {
        #region "Charts Properties"

        private SeriesCollection _chartMinRate = new SeriesCollection(DayConfig);
        public SeriesCollection ChartMinRate
        {
            get => _chartMinRate;
            set
            {
                _chartMinRate = value;
                OnPropertyChanged(nameof(ChartMinRate));
            }
        }
        private SeriesCollection _chartRatesCounter = new SeriesCollection(DayConfig);
        public SeriesCollection ChartRatesCounter
        {
            get => _chartRatesCounter;
            set
            {
                _chartRatesCounter = value;
                OnPropertyChanged(nameof(ChartRatesCounter));
            }
        }
        private SeriesCollection _chartRatesCounterPercent = new SeriesCollection(DayConfig);
        public SeriesCollection ChartRateCountPercent
        {
            get => _chartRatesCounterPercent;
            set
            {
                _chartRatesCounterPercent = value;
                OnPropertyChanged(nameof(ChartRateCountPercent));
            }
        }
        static private CartesianMapper<DateModel> DayConfig { get; } = Mappers.Xy<DateModel>()
                .X(dateModel => dateModel.Date.Ticks / TimeSpan.FromDays(1).Ticks)
                .Y(dateModel => dateModel.Value);

        public Func<double, string> FormatterCounter { get; } = value => value + " кат.";
        public Func<double, string> FormatterCost { get; } = value => value + " руб.";
        public Func<double, string> FormatterPercent { get; } = value => value + " %";

        private Func<double, string> _formatterX = value => value < 0.0 ?
                        new DateTime(0).ToString("d") :
                        new DateTime((long)(value * TimeSpan.FromDays(1).Ticks)).ToString("d");
        public Func<double, string> FormatterX
        {
            get => _formatterX;
            set
            {
                _formatterX = value;
                OnPropertyChanged(nameof(FormatterX));
            }
        }

        #endregion
    }
}
