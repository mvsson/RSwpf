using System;
using System.Collections.Generic;

namespace RateShopperWPF.core
{
    class Rate
    {
        public string Category { get; set; }
        public string Price { get; set; }
        public string Meal { get; set; }

        public string GetPriceLine()
        {
            try
            {
                return $"\t{Price}\t{Category}\n";
            }
            catch (NullReferenceException)
            {
                return "Возникла ошибка, проверьте доступные тарифы на даты\n";
            }
        }
    }
    class RatesByDay
    {
        public DateTime Date { get; set; }
        public List<Rate> Rates { get; set; }
        public string HotelLink { get; set; }

        public RatesByDay()
        {
            Rates = new List<Rate>();
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
}
