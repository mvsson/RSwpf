using System;
using System.Collections.Generic;
using System.Linq;

namespace RateShopperWPF.Models.InputModels
{
    /// <summary>
    /// Хранит в себе вывод RateLine'ов из класса Parser
    /// </summary>
    class DateRates
    {
        public readonly DateTime Date;
        public readonly List<Rate> Rates;
        public readonly string ParentLink;
        public DateRates(string link, DateTime date)
        {
            Rates = new List<Rate>();
            ParentLink = link;
            Date = date;
        }
        public void WithoutAnyRate()
        {
            Rates.Add(new Rate { Price = "Нет доступных номеров" });
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

        public double GetPriceIntegerOrDefault()
        {
            string nums = "0123456789";
            string letters = "абвгдеёжзийклмнопрстуфхцчшщьыъэюя.,";
            string res = string.Empty;
            double price;
            foreach (var ch in Price)
            {
                if (letters.Contains(ch))
                    break;
                if (nums.Contains(ch))
                {
                    res += ch;
                }
            }
            price = res == "" ? 0 : int.Parse(res);
            return price;
        }
    }
}
