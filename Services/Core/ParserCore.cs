﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using RSwpf.Models.InputModels;
using RSwpf.Models.OutputModels;

namespace RSwpf.Services.Core
{
    /// <summary>
    /// Производит загрузку и обрабатывание DOM исходника страницы, получаемой из класса UrlSettings
    /// Выводит данные в класс DateRates
    /// </summary>
    class ParserCore
    {
        /// <summary>
        /// Создаёт массив "DateRates" и заполняет его с помощью .AsParsllel. Отображает процесс на прогрессбар.
        /// </summary>
        /// <param name="progressBar"></param>
        /// <param name="urls"></param>
        /// <returns></returns>
        public async Task<DateRates[]> GetRatesOnDatesAsync(ProgressBarModel progressBar, params UrlModel[] urls)
        {
            var pricesList = new DateRates[urls.Length];

            await Task.WhenAll(
                urls.AsParallel().Select(async (url, index) =>
                {
                    var domDocument = await GetDomPageAsync(url.Link);
                    pricesList[index] = GetDateRates(domDocument, url);
                    progressBar.Value += 1;
                }));
            return pricesList;
        }
        public async Task<DateRates> GetRatesOnDateAsync(ProgressBarModel progressBar, UrlModel url)
        {
            var domDocument = await GetDomPageAsync(url.Link);
            progressBar.Value += 1;
            return GetDateRates(domDocument, url);
        }
        /// <summary>
        /// Создаёт и заполняет экземпляр "DateRates" данными из DOM исходника
        /// </summary>
        /// <param name="document"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateRates GetDateRates(IDocument document, UrlModel url)
        {
            var blocks = GetParseElements(in document, "tr", "js-rt-block-row "); // получаем блоки с категориями номеров и ценами
            DateRates result = new DateRates(url.ParentLink, url.Date);
            if (blocks.Count() == 0)
            {
                result.WithoutAnyRate();
                return result;
            }   
            foreach (var item in blocks)
            {
                var priceLine = new Rate();
                // парсим названия категорий
                var category = GetParseElements(in item, "span", "hprt-roomtype-icon-link ");
                foreach (var _item in category)
                {
                    priceLine.Category = _item.TextContent.Trim();
                }
                // парсим цены
                var price = GetParseElements(in item, "div", "bui-price-display__value prco-inline-block-maker-helper prco-f-font-heading ");
                foreach (var _item in price)
                {
                    priceLine.Price = _item.TextContent.Trim();
                }
                var meal = GetParseElements(in item, "li", "jq_tooltip  rt_clean_up_options");
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
        private IEnumerable<IElement> GetParseElements<T>(in T source, string htmlteg, string htmlclass) where T : IParentNode
        {
            IEnumerable<IElement> blocks = source.QuerySelectorAll(htmlteg).Where(item => item.ClassName != null && item.ClassName.Contains(htmlclass));
            return blocks;
        }
        /// <summary>
        /// Получает DOM искодник по url страницы
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<IDocument> GetDomPageAsync(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url).ConfigureAwait(false);
            return document;
        }
    }
}
