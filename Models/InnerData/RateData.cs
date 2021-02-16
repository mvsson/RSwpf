using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts.Wpf;

namespace RateShopperWPF.Models
{
    /// <summary>
    /// Хранит в себе вывод RateLine'ов из класса Parser
    /// Далее из этого класса собирается вывод в ViewModel
    /// </summary>
    class DateRates
    {
        public DateTime Date { get; }
        public List<Rate> Rates { get; }
        public string ParentLink { get; }
        public DateRates(string link, DateTime date)
        {
            Rates = new List<Rate>();
            ParentLink = link;
            Date = date;
        }
        public void WithoutAnyRate()
        {
            this.Rates.Add(new Rate { Price = "Нет доступных номеров" });
        }
        public GridRateRow[] GetGrid(IDataGridOutput printer)
        {
            return printer.GetGrid(this);
        }
        public static GridRateRow[] GetGrid(IDataGridOutput printer, DateRates[] days)
        {
            return printer.GetGrid(days);
        }
    }
    /// <summary>
    /// Создаётся для вывода из данных из класса Parser
    /// </summary>
    public struct Rate
    {
        public string Category { get; set; }
        public string Price { get; set; }
        public string Meal { get; set; }

        public double GetPriceDoubleOrDefault()
        {
            string nums = "0123456789";
            string res = string.Empty;
            double price;
            foreach (var ch in Price)
            {
                if (".,".Contains(ch))
                    break;
                if (nums.Contains(ch))
                {
                    res += ch;
                }
            }
            price = res == "" ? double.NaN : int.Parse(res);
            return price;
        }
    }
}
