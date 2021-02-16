using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;

namespace RateShopperWPF.ViewModels
{
    public partial class MainViewModel
    {
        #region "Chart Properties"

        private SeriesCollection _chartMinRate = new SeriesCollection();
        public SeriesCollection ChartMinRate
        {
            get => _chartMinRate;
            set
            {
                _chartMinRate = value;
                OnPropertyChanged(nameof(ChartMinRate));
            }
        }
        private SeriesCollection _chartRatesCounter = new SeriesCollection();
        public SeriesCollection ChartRatesCounter
        {
            get => _chartRatesCounter;
            set
            {
                _chartRatesCounter = value;
                OnPropertyChanged(nameof(ChartRatesCounter));
            }
        }
        private SeriesCollection _chartRatesCounterPercent = new SeriesCollection();
        public SeriesCollection ChartRateCountPercent
        {
            get => _chartRatesCounterPercent;
            set
            {
                _chartRatesCounterPercent = value;
                OnPropertyChanged(nameof(ChartRateCountPercent));
            }
        }
        private ObservableCollection<string> _chartLabels = new ObservableCollection<string>();
        public ObservableCollection<string> ChartLabels
        {
            get => _chartLabels;
            set
            {
                _chartLabels = value;
                OnPropertyChanged(nameof(ChartLabels));
            }
        }

        public Func<double, string> FormatterCounter { get; } = value => value + " кат.";
        public Func<double, string> FormatterCost { get; } = value => value + " руб.";
        public Func<double, string> FormatterPercent { get; } = value => value + " %";

        #endregion
    }
}
