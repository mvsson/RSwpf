using System;
using System.Collections.Generic;

namespace RateShopperWPF
{ 
    class UrlOnDate
    {
        public string Link { get; set; }
        public DateTime Date { get; set; }
    }
    class UrlSettings
    {
        public string HotelLink { get; }

        public UrlSettings(string hotelLink)
        {
            HotelLink = hotelLink;
        }

        public string GetHotelPage()
        {
            string _url = $"https://www.booking.com/hotel/ru/{HotelLink}.html";
            return _url;
        }
        public UrlOnDate[] GetUrlsList(in DateSettings range)
        {
            string _url = this.GetHotelPage();
            List<UrlOnDate> result = new List<UrlOnDate>();
            DateTime checkin = range.Start;
            DateTime checkout = checkin.AddDays(1);
            while (checkin < range.End)
            {
                var linkDate = new UrlOnDate();
                linkDate.Link = _url + $"?checkin={checkin:yyyy-MM-dd};checkout={checkout:yyyy-MM-dd}";
                linkDate.Date = checkin;
                result.Add(linkDate);
                checkin = checkin.AddDays(range.Step);
                checkout = checkout.AddDays(range.Step);
            }
            return result.ToArray();
        }
    }
}
