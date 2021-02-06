using System;
using System.Collections.Generic;

namespace RateShopperWPF.core
{
    class PriceByDay
    {
        public DateTime Date { set; get; }
        public List<PriceLine> Rates { set; get; }

        public PriceByDay()
        {
            Rates = new List<PriceLine>();
        }
    }

    class PriceLine
    {
        public string Category { set; get; }
        public string Price { set; get; }
        public string Meal { set; get; }
    }
}
