using System;
using RateShopperWPF.Models.InputModels;

namespace RateShopperWPF.Models.OutputModels
{
    public struct GridRowModel
    {
        public DateTime Date { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string Meal { get; set; }
        public string ParentLink { get; set; }
        public GridRowModel(DateTime date, Rate priceLine, string link)
        {
            Date = date;//.ToString("yyyy.MM.dd");
            Price = priceLine.Price;
            Category = priceLine.Category;
            Meal = priceLine.Meal;
            ParentLink = link;
        }
    }
}
