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
    class Parser
    {
        /// <summary>
        /// Создаёт массив "PriceByDay" и заполняет его с помощью .AsParsllel. Отображает процесс на прогрессбар.
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="urls"></param>
        /// <returns></returns>
        public static async Task<RatesByDay[]> GetRatesListAsync(ProgressBar progressBar, UrlOnDate[] urls)
        {
            var pricesList = new RatesByDay[urls.Length];

            await Task.WhenAll(
                urls.AsParallel().Select(async (url, index) =>
                {
                    var domDocument = await GetDomPageAsync(url.Link);
                    pricesList[index] = GetRatesByDay(domDocument, url.Date);
                    Application.Current.Dispatcher.Invoke(() => progressBar.Value += 1);
                }));
            return pricesList;
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
        /// <summary>
        /// Создаёт и заполняет экземпляр "PriceByDay" данными из DOM исходника
        /// </summary>
        /// <param name="document"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private static RatesByDay GetRatesByDay(IDocument document, DateTime date)
        {
            var blocks = GetParse(in document, "tr", "js-rt-block-row "); // получаем блоки с категориями номеров и ценами
            RatesByDay result = new RatesByDay { Date = date };

            foreach (var item in blocks)
            {
                var priceLine = new Rate();
                // парсим названия категорий
                var category = GetParse(in item, "span", "hprt-roomtype-icon-link ");
                foreach (var _item in category)
                {
                    priceLine.Category = _item.TextContent.Trim();
                }
                // парсим цены
                var price = GetParse(in item, "div", "bui-price-display__value prco-inline-block-maker-helper prco-font16-helper ");
                foreach (var _item in price)
                {
                    priceLine.Price = _item.TextContent.Trim();
                }
                var meal = GetParse(in item, "li", "jq_tooltip  rt_clean_up_options");
                foreach (var _item in meal)
                {
                    priceLine.Meal = _item.TextContent.Trim();
                }
                result.Rates.Add(priceLine);
            }
            return result;
        }
        /// <summary>
        /// Отсеивает необходимые строки для получения данных о тарифах из "source"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Источник для парсинга</param>
        /// <param name="htmlteg"></param>
        /// <param name="htmlclass">Класс в теге</param>
        /// <returns></returns>
        private static IEnumerable<IElement> GetParse<T>(in T source, string htmlteg, string htmlclass) where T : IParentNode
        {
            IEnumerable<IElement> blocks = source.QuerySelectorAll(htmlteg).Where(item => item.ClassName != null && item.ClassName.Contains(htmlclass));
            return blocks;
        }
        /// <summary>
        /// Получает DOM искодник по url страницы
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static async Task<IDocument> GetDomPageAsync(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);
            return document;
        }
    }
}
