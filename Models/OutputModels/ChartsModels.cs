using System;
using LiveCharts;
using LiveCharts.Wpf;

namespace RSwpf.Models.OutputModels
{
    class ChartsModel
    {
        public readonly LineSeries ChartMinRate;
        public readonly LineSeries ChartRatesCounter;
        public readonly LineSeries ChartRatesCounterPercent;

        public ChartsModel(string parentLink, bool isShowLabels)
        {
            ChartMinRate = new LineSeries() 
            { 
                Title = parentLink, 
                Values = new ChartValues<PointModel>(),
                DataLabels = isShowLabels
            };
            ChartRatesCounter = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<PointModel>(),
                DataLabels = isShowLabels
            };
            ChartRatesCounterPercent = new LineSeries()
            {
                Title = parentLink,
                Values = new ChartValues<PointModel>(),
                DataLabels = isShowLabels
            };
        }
    }
    public struct PointModel
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }

}
