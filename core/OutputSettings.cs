using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace RateShopperWPF.core
{
    class GridCollection
    {
        public ObservableCollection<DataGridRateRow> Source { get; }
        public GridCollection()
        {
            Source = new ObservableCollection<DataGridRateRow>();
        }
        public void AddRange(IList<DataGridRateRow> source)
        {
            source.ToList().ForEach(item => Source.Add(item));
        }
        public void Add(DataGridRateRow item)
        {
            Source.Add(item);
        }
    }
    struct DataGridRateRow
    {
        public string Date { get; }
        public string Price { get; }
        public string Category { get; }
        public string Meal { get; }
        public DataGridRateRow(DateTime date, Rate priceLine)
        {
            Date = date.ToString("yyyy.MM.dd");
            Price = priceLine.Price;
            Category = priceLine.Category;
            Meal = priceLine.Meal;
        }
    }
    interface IDataGridOutput
    {
        DataGridRateRow[] GetGrid(params RatesByDay[] days);
    }

    interface ITextBoxOutput
    {
        string GetText(params RatesByDay[] days);
    }
    interface IOutput : IDataGridOutput, ITextBoxOutput { }

    class OutputShort : IOutput
    {
        public DataGridRateRow[] GetGrid(params RatesByDay[] days)
        {
            
            var output = days.Select(day => new DataGridRateRow(day.Date, day.Rates[0])).ToArray();
            return output;
        }
        public string GetText(params RatesByDay[] days)
        {
            string output = string.Empty;
            foreach(var day in days)
            {
                output += day.Date.ToShortDateString() + day.Rates[0].GetPriceLine();
            }
            return output;
        }
    }

    class OutputDetailed : IOutput
    {
        public DataGridRateRow[] GetGrid(params RatesByDay[] days)
        {
            var output = new List<DataGridRateRow>();            
            foreach (var day in days)
            {
                output.AddRange(day.Rates.Where(rate => rate.Category != null).Select(rate => new DataGridRateRow(day.Date, rate)));
            }

            return output.ToArray();
        }
        public string GetText(params RatesByDay[] days)
        {
            string output = string.Empty;
            foreach (var day in days)
            {
                output += day.Date.ToShortDateString() + "\n";
                day.Rates.ForEach(rate => output += rate.GetPriceLine());
            }
            return output;
        }
    }
}
