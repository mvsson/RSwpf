using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateShopperWPF
{
    class UrlSettings
    {
        public string HotelLink { get; }

        public UrlSettings(string hotelLink)
        {
            HotelLink = hotelLink;
        }

        public string getHotelPage()
        {
            string _url = $"https://www.booking.com/hotel/ru/{HotelLink}.html";
            return _url;
        }
        public string[] getUrlsList(in DateSettings range)
        {
            string _url = this.getHotelPage();
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
