using System;
using System.Collections.Generic;
using System.Linq;

namespace RateShopperWPF.Services.Core
{
    class DatesCreator
    {
        private DateTime StartRange { get; set; }
        private readonly DateTime EndRange;
        public DatesCreator(DateTime startRange, DateTime endRange)
        {
            StartRange = startRange;
            EndRange = endRange;
        }

        public DateTime[][] GetSplitDateList(int lenght = 16)
        {
            var dateList = GetCheckinList();
            int i = 0;
            var items = from s in dateList
                        let num = i++
                        group s by num / lenght into g
                        select g.ToArray();
            return items.ToArray();
        }

        private DateTime[] GetCheckinList()
        {
            var dateList = new List<DateTime>();
            while(StartRange < EndRange)
            {
                dateList.Add(StartRange);
                StartRange = StartRange.AddDays(1);
            }
            return dateList.ToArray();
        }
    }
}
