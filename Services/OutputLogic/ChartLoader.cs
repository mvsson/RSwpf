using System.Linq;
using RSwpf.Models.InputModels;
using RSwpf.Models.OutputModels;

namespace RSwpf.Services.OutputLogic
{
    class ChartLoader
    {
        private readonly double MaxCountCategory;
        public ChartLoader(double maxCount)
        {
            MaxCountCategory = maxCount;
        }

        public PointModel AddPointMinRate(DateRates day)
        {
            return new PointModel()
            {
                Value = day.Rates[0].GetPriceIntegerOrDefault(),
                Date = day.Date
            };
        }
        public PointModel AddPointRatesCounter(DateRates day)
        {
            int count = day.Rates.Where(rate => rate.Category != null).Count();
            return new PointModel()
            {
                Value = count,
                Date = day.Date
            };
        }
        public PointModel AddPointRatesCountPercent(DateRates day)
        {
            if (MaxCountCategory == 0)
                return new PointModel();

            int count = day.Rates.Where(rate => rate.Category != null).Count();
            return new PointModel()
            {
                Value = (int)(count / MaxCountCategory * 100),
                Date = day.Date
            };     
        }
    }
}
