using System;
using System.Collections.Generic;

namespace RateShopperWPF.core
{
    interface IGetterPriceLine
    {
        string GetPriceLine();
    }

    class PriceLine : IGetterPriceLine
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

    class PriceByDay : IGetterPriceLine
    {
        public DateTime Date { get; set; }
        public List<PriceLine> Rates { get; set; }
        public string HotelLink { get; set; }

        public PriceByDay()
        {
            Rates = new List<PriceLine>();
        }
        public string GetPriceLine()  // возвращает минимальный тариф на дату, находящийся под индексом [0]
        {
            string priceLine = Date.ToString("dd-MM-yyyy") + "\t" + Rates[0].GetPriceLine();
            return priceLine;
        }
        public string GetAllPrices()
        {
            string priceBlock = "\t" + Date.ToString("dd-MM-yyyy") + "\n";
            Rates.ForEach(rate => priceBlock += rate.GetPriceLine());
            return priceBlock;
        }
    }
}
