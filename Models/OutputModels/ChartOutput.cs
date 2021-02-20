using System;
using LiveCharts;
using LiveCharts.Wpf;

namespace RateShopperWPF.Models.OutputModels
{
    class ChartsModel
    {
        public readonly LineSeries ChartMinRate;
        public readonly LineSeries ChartRatesCounter;
        public readonly LineSeries ChartRatesCounterPercent;
        public readonly ColumnSeries ChartColumn;

        public ChartsModel(string parentLink, bool showLabels)
        {
            ChartMinRate = new LineSeries() 
            { 
                Title = parentLink, 
                Values = new ChartValues<DateModel>(),
                DataLabels = showLabels
            };
            ChartRatesCounter = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<DateModel>(),
                DataLabels = showLabels
            };
            ChartRatesCounterPercent = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<DateModel>(),
                DataLabels = showLabels
            };
            ChartColumn = new ColumnSeries()
            {
                Values = new ChartValues<DateModel>()
            };
        }
    }
    public struct DateModel
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }

}
