using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AngleSharp;
using AngleSharp.Dom;
using RateShopperWPF.core;

namespace RateShopperWPF
{
    class PriceParser
    {
        public static void ShowOnBoardPrices(UrlSettings hotelUrlSettings, TextBox output, PriceByDay[] dayList)
        {
            output.Text += "Минимальные цены в отеле " + hotelUrlSettings.HotelLink + ", на даты:" + "\n";
            
            foreach (var day in dayList)
            {
                try
                {
                    output.Text += day.Date.ToString("yyyy-MM-dd") + "\t";
                    output.Text += day.Rates[0].Price + "\t" + day.Rates[0].Category + "\n";// под индексом 0 самый дешевый тариф
                }
                catch
                {
                    output.Text += "Возникла ошибка, проверьте доступные тарифы на даты\n";
                }
            }
        }
        public static void ShowOnBoardPricesDetailed(UrlSettings hotelUrlSettings, TextBox output, PriceByDay[] dayList)
        {
            foreach (var day in dayList)
            {
                try
                {
                    output.Text += "\n\tЦена на дату: " + day.Date.ToString("yyyy-MM-dd") + $" В отеле [{hotelUrlSettings.HotelLink}]" + "\n";
                    foreach (var rate in day.Rates)
                        if (rate.Category != null) // если null, значит категория под предыдущим индексом
                            output.Text += rate.Price + "\t" + rate.Category + "\n";
                }
                catch
                {
                    output.Text += "Возникла ошибка, проверьте доступные тарифы на даты\n";
                }
            }
        }

        public static async Task<PriceByDay[]> GetPricesListAsync(ProgressBar progressBar, string[] urls, DateTime start)
        {
            Application.Current.Dispatcher.Invoke(() => progressBar.Maximum = urls.Length);
            var pricesList = new PriceByDay[urls.Length];

            await Task.WhenAll(
                urls.AsParallel().Select(async (url, index) =>
                {
                    var domDocument = await GetDomPageAsync(url);
                    pricesList[index] = GetPricesByDay(domDocument, start.AddDays(index));
                    Application.Current.Dispatcher.Invoke(() => progressBar.Value += 1);
                }));
            
            return pricesList;
        }

        private static PriceByDay GetPricesByDay(IDocument document, DateTime date)  ///////////////////////////////////////////////////////////
        {            
            var blocks = GetParse(in document, "tr", "js-rt-block-row "); // получаем блоки с категориями номеров и ценами
            PriceByDay result = new PriceByDay { Date = date };

            foreach (var item in blocks)
            {
                var priceLine = new PriceLine ();
                // парсим названия категорий
                var category = GetParse(in item, "span", "hprt-roomtype-icon-link ");
                foreach (var item_ in category)
                { 
                    priceLine.Category = item_.TextContent.Trim(); 
                }
                // парсим цены
                var price = GetParse(in item, "div", "bui-price-display__value prco-inline-block-maker-helper prco-font16-helper ");
                foreach (var item_ in price)
                { 
                    priceLine.Price = item_.TextContent.Trim(); 
                }
                result.Rates.Add(priceLine);
            }
            return result;
        }

        private static async Task<IDocument> GetDomPageAsync(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);
            return document;
        }

        private static IEnumerable<IElement> GetParse<T>(in T source, string htmlteg, string htmlclass) where T : IParentNode
        {
            IEnumerable<IElement> blocks = source.QuerySelectorAll(htmlteg).Where(item => item.ClassName != null && item.ClassName.Contains(htmlclass));
            return blocks;
        }
    }
}
