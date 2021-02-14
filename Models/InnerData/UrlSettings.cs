using System;
using System.Collections.Generic;
using System.Linq;

namespace RateShopperWPF.Models
{
    /// <summary>
    /// Создает список страниц привязанных к дате
    /// </summary>
    class UrlOnDate
    {
        public string BaseLink { get; set; }
        public string Link { get; set; }
        public DateTime Date { get; set; }
        public UrlOnDate( string link )
        {
            BaseLink = link;
        }
    }
    class UrlSettings
    {
        private string HotelLink { get; }
        private UrlSettings(string hotelLink)
        {
            HotelLink = hotelLink;
        }

        private string GetHotelPage()
        {
            string _url = $"https://www.booking.com/hotel/ru/{HotelLink}.html";
            return _url;
        }
        public static UrlOnDate[] GetUrlsList(DateTime startParse, DateTime endParse, string link)
        {
            UrlSettings hotelUrlSettings = new UrlSettings(link);
            DateSettings parsingDates = new DateSettings(startParse, endParse);
            UrlOnDate[] urls = hotelUrlSettings._GetUrlsList(parsingDates);
            return urls;
        }
        private UrlOnDate[] _GetUrlsList(in DateSettings range)
        {
            string _url = GetHotelPage();
            List<UrlOnDate> result = new List<UrlOnDate>();
            DateTime checkin = range.Start;
            while (checkin < range.End)
            {
                var linkDate = new UrlOnDate(HotelLink);
                linkDate.Link = _url + $"?checkin={checkin:yyyy-MM-dd};checkout={checkin.AddDays(1):yyyy-MM-dd}";
                linkDate.Date = checkin;
                result.Add(linkDate);
                checkin = checkin.AddDays(range.PagesStep);
            }
            return result.ToArray();
        }
        /// <summary>
        /// Разделяет массив "urls" на массивы длинной по "lenght".
        /// Необходим для обхода разрыва соединения со стороны сервера.
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="lenght">
        /// Для большей производительности значение должно быть кратно количеству потоков процессора, 
        /// но не меньше двухкратного значения 
        /// </param>
        /// <returns></returns>
        public static UrlOnDate[][] SplitUrlsListByN(UrlOnDate[] urls, int lenght = 16)
        {
            int i = 0;
            var items = from s in urls
                        let num = i++
                        group s by num / lenght into g
                        select g.ToArray();
            return items.ToArray();
        }
    }
}
