using System;
using RateShopperWPF.Models.InputModels;

namespace RateShopperWPF.Services.Core
{
    class UrlsCreator
    {
        private readonly string ParentLink;
        const string BaseUrl = "https://www.booking.com/hotel/ru/";

        public UrlsCreator(string hotelLink)
        {
            ParentLink = hotelLink;
        }

        public UrlModel GetUrl(DateTime checkIn)
        {
            string link = $"{BaseUrl}{ParentLink}.html?checkin={checkIn:yyyy-MM-dd};checkout={checkIn.AddDays(1):yyyy-MM-dd}";

            return new UrlModel(ParentLink, link, checkIn);
        }
    }
}
