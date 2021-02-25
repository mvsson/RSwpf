using System;
using System.Collections.Generic;
using System.Linq;

namespace RSwpf.Services.Core
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

        public DateTime[][] GetSplitDates(int lenght = 16)
        {
            var dates = GetCheckinDates();
            int i = 0;
            var items = from s in dates
                        let num = i++
                        group s by num / lenght into g
                        select g.ToArray();
            return items.ToArray();
        }

        private DateTime[] GetCheckinDates()
        {
            var dates = new List<DateTime>();
            while(StartRange < EndRange)
            {
                dates.Add(StartRange);
                StartRange = StartRange.AddDays(1);
            }
            return dates.ToArray();
        }
    }
}
