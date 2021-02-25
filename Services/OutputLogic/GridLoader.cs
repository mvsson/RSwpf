using System.Collections.Generic;
using System.Linq;
using RSwpf.Models.InputModels;
using RSwpf.Models.OutputModels;

namespace RSwpf.Services.OutputLogic
{
    class GridLoader
    {
        private readonly IGridLoader printer;
        public GridLoader(bool isShowDetailed)
        {
            printer = isShowDetailed ? printer = new GridLoaderDetailed() : new GridLoaderShort();
        }
        public GridRowModel[] GetRows(DateRates[] days)
        {
            return printer.GetRows(days);
        }
    }

    interface IGridLoader
    {
        GridRowModel[] GetRows(params DateRates[] days);
    }

    class GridLoaderDetailed : IGridLoader
    {
        public GridRowModel[] GetRows(params DateRates[] days)
        {
            var output = new List<GridRowModel>();
            foreach (var day in days)
            {
                output.AddRange(day.Rates
                    .Where(rate => rate.Category != null)
                    .Select(rate => new GridRowModel(day.Date, rate, day.ParentLink)));
            }
            return output.ToArray();
        }
    }
    class GridLoaderShort : IGridLoader
    {
        public GridRowModel[] GetRows(params DateRates[] days)
        {
            GridRowModel[] output = new GridRowModel[days.Length];
            for (int i = 0; i < days.Length; i++)
            {
                output[i] = new GridRowModel(days[i].Date, days[i].Rates[0], days[i].ParentLink);
            }
            return output;
        }
    }
}
