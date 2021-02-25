using System;
using RSwpf.Models.InputModels;

namespace RSwpf.Services.Core
{
    class UrlCreator
    {
        private readonly string ParentLink;
        const string BaseUrl = "https://www.booking.com/hotel/ru/";

        public UrlCreator(string hotelLink)
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
