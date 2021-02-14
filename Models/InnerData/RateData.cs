using System;
using System.Collections.Generic;
using RateShopperWPF.ViewModels;

namespace RateShopperWPF.Models
{
    /// <summary>
    /// Хранит в себе вывод RateLine'ов из класса Parser
    /// Далее из этого класса собирается вывод в ViewModel
    /// </summary>
    class RatesByDay
    {
        public DateTime Date { get; }
        public List<RateLine> Rates { get; }
        public string RarentLink { get; }
        public RatesByDay(string link, DateTime date)
        {
            Rates = new List<RateLine>();
            RarentLink = link;
            Date = date;
        }
        public void WithoutAnyRate()
        {
            this.Rates.Add(new RateLine { Category = "Нет доступных номеров" });
        }
        public string GetText(IOutput printer)
        {
            return printer.GetText(this);
        }
        public DataGridRateRow[] GetGrid(IOutput printer)
        {
            return printer.GetGrid(this);
        }
        public static string GetText(IOutput printer, RatesByDay[] days)
        {
            return printer.GetText(days);
        }
        public static DataGridRateRow[] GetGrid(IOutput printer, RatesByDay[] days)
        {
            return printer.GetGrid(days);
        }
    }
    /// <summary>
    /// Создаётся для вывода из данных из класса Parser
    /// </summary>
    public struct RateLine
    {
        public string Category { get; set; }
        public string Price { get; set; }
        public string Meal { get; set; }

        public string GetLine()
        {
            return $"\t{Price}\t{Meal}\t{Category}\n";
        }
    }
}
