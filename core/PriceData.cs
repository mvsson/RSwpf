using System;
using System.Collections.Generic;

namespace RateShopperWPF.core
{
    class PriceLine
    {
        public string Category { get; set; }
        public string Price { get; set; }
        public string Meal { get; set; }

        public string GetPriceLine()
        {
            try
            {
                return $"{Price}\t{Category}\n";
            }
            catch (NullReferenceException)
            {
                return "Возникла ошибка, проверьте доступные тарифы на даты\n";
            }
        }
    }

    class PriceByDay
    {
        public DateTime Date { get; set; }
        public List<PriceLine> Rates { get; set; }
        public string HotelLink { get; set; }

        public PriceByDay()
        {
            Rates = new List<PriceLine>();
        }
        public string GetShortPriceText()  // возвращает минимальный тариф на дату, находящийся под индексом [0]
        {
            string priceLine = Date.ToString("dd-MM-yyyy") + "\t" + Rates[0].GetPriceLine();
            return priceLine;
        }
        public string GetDetailedPriceText()
        {
            string priceBlock = "\t" + Date.ToString("dd-MM-yyyy") + "\n";
            Rates.ForEach(rate => priceBlock += "   " + rate.GetPriceLine());
            return priceBlock;
        }
    }
}
