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

        public ChartsModel(string parentLink, bool showLabels)
        {
            ChartMinRate = new LineSeries() 
            { 
                Title = parentLink, 
                Values = new ChartValues<PointModel>(),
                DataLabels = showLabels
            };
            ChartRatesCounter = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<PointModel>(),
                DataLabels = showLabels
            };
            ChartRatesCounterPercent = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<PointModel>(),
                DataLabels = showLabels
            };
        }
    }
    public struct PointModel
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }

}
