using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RateShopperWPF.Models
{
    /// <summary>
    /// Хранит в себе значения для вывода рядов в таблицу View
    /// </summary>
    public struct GridRateRow
    {
        public DateTime Date { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string Meal { get; set; }
        public string ParentLink { get; set; }
        public GridRateRow(DateTime date, Rate priceLine, string link)
        {
            Date = date;//.ToString("yyyy.MM.dd");
            Price = priceLine.Price;
            Category = priceLine.Category;
            Meal = priceLine.Meal;
            ParentLink = link;
        }
    }
    interface IDataGridOutput
    {
        GridRateRow[] GetGrid(params DateRates[] days);
    }

    class OutputShort : IDataGridOutput
    {
        public GridRateRow[] GetGrid(params DateRates[] days)
        {
            GridRateRow[] output = new GridRateRow[days.Length];
            for (int i = 0; i < days.Length; i++)
            {
                output[i] = new GridRateRow(days[i].Date, days[i].Rates[0], days[i].ParentLink);
            }
            return output;
        }
    }

    class OutputDetailed : IDataGridOutput
    {
        public GridRateRow[] GetGrid(params DateRates[] days)
        {
            var output = new List<GridRateRow>();
            foreach (var day in days)
            {
                output.AddRange(day.Rates
                    .Where(rate => rate.Category != null)
                    .Select(rate => new GridRateRow(day.Date, rate, day.ParentLink)));
            }
            return output.ToArray();
        }
    }
}
