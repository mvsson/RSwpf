using System.Linq;
using RateShopperWPF.Models.InputModels;
using RateShopperWPF.Models.OutputModels;

namespace RateShopperWPF.Services.VMLoaders
{
    class ChartLoader
    {
        public readonly ChartsModel Charts;
        public ChartLoader(string parentLink, bool showLabels)
        {
            Charts = new ChartsModel(parentLink, showLabels);
        }
        public void FillCharts(DateRates[] days, double maxCount)
        {
            foreach (var day in days)
            {
                AddChartMinRate(day);
                AddChartsRatesCounter(day, maxCount);
                AddChartColumn(day, 1);
            }
        }
        private void AddChartColumn(DateRates day, int i)
        {
            Charts.ChartColumn.Values.Add(new DateModel()
            {
                Date = day.Date,
                Value = i,
            });
        }
        private void AddChartMinRate(DateRates day)
        {
            Charts.ChartMinRate.Values.Add(new DateModel()
            {
                Value = day.Rates[0].GetPriceIntegerOrDefault(),
                Date = day.Date
            });
        }
        private void AddChartsRatesCounter(DateRates day, double maxCount)
        {
            int count = day.Rates.Where(rate => rate.Category != null).Count();
            Charts.ChartRatesCounter.Values.Add(new DateModel()
            {
                Value = count,
                Date = day.Date
            });
            if (maxCount == 0)
                return;
            Charts.ChartRatesCounterPercent.Values.Add(new DateModel()
            {
                Value = (int)(count / maxCount * 100),
                Date = day.Date
            });
        }
    }

}
