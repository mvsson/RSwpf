using System;
using System.Collections.Generic;
using System.Linq;

namespace RateShopperWPF.Models
{
    /// <summary>
    /// Наследует от себя OutputShort и OutputDetailed принтеры для методов GetGrid и GetText класса RatesByDay
    /// </summary>
    interface IOutput : IDataGridOutput, ITextBoxOutput { }
    interface IDataGridOutput
    {
        DataGridRateRow[] GetGrid(params RatesByDay[] days);
    }

    interface ITextBoxOutput
    {
        string GetText(params RatesByDay[] days);
    }

    class OutputShort : IOutput
    {
        public DataGridRateRow[] GetGrid(params RatesByDay[] days)
        {
            DataGridRateRow[] output = new DataGridRateRow[days.Length];
            for (int i=0; i<days.Length; i++)
            {
                output[i] = new DataGridRateRow(days[i].Date, days[i].Rates[0], days[i].RarentLink);
            }
            return output;
        }
        public string GetText(params RatesByDay[] days)
        {
            string output = string.Empty;
            foreach (var day in days)
            {
                output += day.Date.ToShortDateString() + day.Rates[0].GetLine();
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
                output.AddRange(day.Rates
                    .Where(rate => rate.Category != null)
                    .Select(rate => new DataGridRateRow(day.Date, rate, day.RarentLink)));
            }
            return output.ToArray();
        }
        public string GetText(params RatesByDay[] days)
        {
            string output = string.Empty;
            foreach (var day in days)
            {
                output += day.Date.ToShortDateString() + "\n";
                day.Rates.Where(rate => rate.Category!=null).ToList()
                    .ForEach(rate => output += rate.GetLine());
            }
            return output;
        }
    }
}
