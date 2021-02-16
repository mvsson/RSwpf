using System.Collections.Generic;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;

namespace RateShopperWPF.Models
{
    class ChartLoader
    {
        public List<string> Labels { get; private set; }
        public LineSeries ChartMinRate { get; private set; }
        public LineSeries ChartRatesCounter { get; private set; }
        public LineSeries ChartRatesCounterPercent { get; private set; }

        public ChartLoader(string parentLink)
        {
            Labels = new List<string>();
            ChartMinRate = new LineSeries() 
            { 
                Title = parentLink, 
                Values = new ChartValues<double>(),
                DataLabels = true
            };
            ChartRatesCounter = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<int>(),
                DataLabels = true
            };
            ChartRatesCounterPercent = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<int>(),
                DataLabels = true
            };
        }
        public void FillCharts(DateRates[] days, double maxCount)
        {
            foreach(var day in days)
            {
                AddChartMinRate(day);
                AddChartsRatesCounter(day, maxCount);
            }
        }
        private void AddChartMinRate(DateRates day)
        {
            Labels.Add(day.Date.ToShortDateString());
            ChartMinRate.Values.Add(day.Rates[0].GetPriceDoubleOrDefault());
        }
        private void AddChartsRatesCounter(DateRates day, double maxCount)
        {
            int count = day.Rates.Where(rate => rate.Category != null).Count();
            ChartRatesCounter.Values.Add(count);
            ChartRatesCounterPercent.Values.Add((int)(count / maxCount * 100));
        }
    }
}
