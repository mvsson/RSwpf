using System.Collections.Generic;
using System.Linq;
using RateShopperWPF.Models.InputModels;
using RateShopperWPF.Models.OutputModels;

namespace RateShopperWPF.Services.VMLoaders
{
    class GridLoader
    {
        private readonly IGridLoader Loader;
        public GridLoader(IGridLoader loader)
        {
            Loader = loader;
        }

        public GridRateRow[] GetGrid(DateRates[] days)
        {
            return Loader.GetGrid(days);
        }
    }

    interface IGridLoader
    {
        GridRateRow[] GetGrid(params DateRates[] days);
    }

    class GridLoaderDetailed : IGridLoader
    {
        public GridRateRow[] GetGrid(params DateRates[] days)
        {
            var output = new List<GridRateRow>();
            foreach (var day in days)
            {
                output.AddRange(day.Rates
                    .Where(rate => rate.Category != null)
                    .Select(rate => new GridRateRow(day.Date, rate, day.ParentLink)));
            }
            return output.ToArray();
        }
    }
    class GridLoaderShort : IGridLoader
    {
        public GridRateRow[] GetGrid(params DateRates[] days)
        {
            GridRateRow[] output = new GridRateRow[days.Length];
            for (int i = 0; i < days.Length; i++)
            {
                output[i] = new GridRateRow(days[i].Date, days[i].Rates[0], days[i].ParentLink);
            }
            return output;
        }
    }
}
