using System;

namespace RateShopperWPF
{
    class DateSettings
    {
        public DateTime Start { get; } = DateTime.Today;
        public DateTime End { get; } = DateTime.Today.AddDays(1);
        public int PagesStep { get; }

        public DateSettings(in DateTime start, in DateTime end, int parseStep = 1)
        {
            Start = start;
            End = end;
            PagesStep = parseStep;
        }
    }
}
