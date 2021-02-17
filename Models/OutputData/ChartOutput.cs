using System;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;

namespace RateShopperWPF.Models
{
    class ChartLoader
    {
        public Func<double, string> FormatterX { get; private set; }
        public LineSeries ChartMinRate { get; private set; }
        public LineSeries ChartRatesCounter { get; private set; }
        public LineSeries ChartRatesCounterPercent { get; private set; }
        public ColumnSeries ChartColumn { get; private set; }

        public ChartLoader(string parentLink)
        {
            FormatterX = value => new DateTime((long)(value * TimeSpan.FromDays(1).Ticks)).ToString("d");
            
            ChartMinRate = new LineSeries() 
            { 
                Title = parentLink, 
                Values = new ChartValues<DateModel>(),
                DataLabels = true
            };
            ChartRatesCounter = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<DateModel>(),
                DataLabels = true
            };
            ChartRatesCounterPercent = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<DateModel>(),
                DataLabels = true
            };
            ChartColumn = new ColumnSeries()
            {
                Values = new ChartValues<DateModel>()
            };
        }
        public void FillCharts(DateRates[] days, double maxCount)
        {
            foreach(var day in days)
            {
                AddChartMinRate(day);
                AddChartsRatesCounter(day, maxCount);
                AddChartColumn(day, 1);
            }
        }
        private void AddChartColumn(DateRates day, int i)
        {
            ChartColumn.Values.Add(new DateModel()
            {
                Date = day.Date,
                Value = i,
            });
        }
        private void AddChartMinRate(DateRates day)
        {
            ChartMinRate.Values.Add(new DateModel()
            {
                Value = day.Rates[0].GetPriceIntegerOrDefault(),
                Date = day.Date
            });
        }
        private void AddChartsRatesCounter(DateRates day, double maxCount)
        {
            int count = day.Rates.Where(rate => rate.Category != null).Count();
            ChartRatesCounter.Values.Add(new DateModel()
            {
                Value = count,
                Date = day.Date
            });
            if (maxCount == 0)
                return;
            ChartRatesCounterPercent.Values.Add(new DateModel()
            { 
                Value = (int)(count / maxCount * 100),
                Date = day.Date
            });
        }
    }
    public class DateModel
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }

}
