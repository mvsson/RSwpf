using System;
using RateShopperWPF.Models.InputModels;

namespace RateShopperWPF.Models.OutputModels
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
}
