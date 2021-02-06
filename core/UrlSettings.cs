using System;
using System.Collections.Generic;

namespace RateShopperWPF
{
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
        public string[] GetUrlsList(in DateSettings range)
        {
            string _url = this.GetHotelPage();
            List<string> result = new List<string>();
            DateTime checkin = range.Start;
            DateTime checkout = checkin.AddDays(1);
            while (checkin < range.End)
            {
                result.Add(_url + $"?checkin={checkin:yyyy-MM-dd};checkout={checkout:yyyy-MM-dd}");
                checkin = checkin.AddDays(range.Step);
                checkout = checkout.AddDays(range.Step);
            }
            return result.ToArray();
        }
    }
}
