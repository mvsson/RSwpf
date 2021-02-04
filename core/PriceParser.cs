using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AngleSharp;
using AngleSharp.Dom;


namespace RateShopperWPF
{
    class PriceParser
    {
        public static async Task ShowQuickOutputPricesAsync(string[] urls, ProgressBar progressBar, bool showDetailed,
            UrlSettings hotelUrlSettings, DateSettings parsingDates, TextBox output)
        {
            var pricesList = new List<List<List<string>>>(urls.Length);

            if (!showDetailed)
                output.Text += "Минимальные цены в отеле " + hotelUrlSettings.HotelLink + ", на даты:" + "\n";
            DateTime dateCount = parsingDates.Start;

            await Task.WhenAll(urls.AsParallel().AsOrdered().Select(async url =>
            {
                var _ = await GetDayPricesAsync(url);

                try
                {
                    if (showDetailed)
                    {
                        Application.Current.Dispatcher.Invoke(() => 
                            output.Text += "\n\tЦена на дату: " + dateCount.ToString("yyyy-MM-dd") + $" В отеле [{hotelUrlSettings.HotelLink}]" + "\n");
                        for (int ii = 0; ii < _.Count; ii++) // перебираем все цены на дату
                            if (_[ii].Count > 1) // если больше 1 значения, значит в блоке под индексом 0 - название, 1 - цена
                                Application.Current.Dispatcher.Invoke(() =>
                                    output.Text += _[ii][1] + "\t" + _[ii][0] + "\n");
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            output.Text += dateCount.ToString("yyyy-MM-dd") + "\t";
                            output.Text += _[0][1] + "\t" + _[0][0] + "\n";
                        });
                    }
                }
                catch
                {
                    Application.Current.Dispatcher.Invoke(() =>
                        output.Text += "Возникла ошибка, проверьте наличие цен на выбранные даты\n");
                }
                dateCount = dateCount.AddDays(parsingDates.Step);

                Application.Current.Dispatcher.Invoke(() => progressBar.Value += 1);
            }));
        }



        public static async Task ShowOutputPricesAsync(string[] urls, ProgressBar progressBar, bool showDetailed, 
            UrlSettings hotelUrlSettings, DateSettings parsingDates, TextBox output)
        {
            var pricesList = await GetPricesListAsync(urls, progressBar);

            if (!showDetailed)
                output.Text += "Минимальные цены в отеле " + hotelUrlSettings.HotelLink + ", на даты:" + "\n";
            DateTime dateCount = parsingDates.Start;
            for (int i = 0; i < urls.Length; i++)  // перебираем даты
            {
                try
                {
                    if (showDetailed)
                    {
                        output.Text += "\n\tЦена на дату: " + dateCount.ToString("yyyy-MM-dd") + $" В отеле [{hotelUrlSettings.HotelLink}]" + "\n";
                        for (int ii = 0; ii < pricesList[i].Count; ii++) // перебираем все цены на дату
                            if (pricesList[i][ii].Count > 1) // если больше 1 значения, значит в блоке под индексом 0 - название, 1 - цена
                                output.Text += pricesList[i][ii][1] + "\t" + pricesList[i][ii][0] + "\n";
                    }
                    else
                    {
                        output.Text += dateCount.ToString("yyyy-MM-dd") + "\t";
                        output.Text += pricesList[i][0][1] + "\t" + pricesList[i][0][0] + "\n";
                    }
                }
                catch
                {
                    output.Text += "Возникла ошибка, проверьте наличие цен на выбранные даты\n";
                }
                dateCount = dateCount.AddDays(parsingDates.Step);
            }
        }

        public static async Task<List<List<List<string>>>> GetPricesListAsync(string[] urls, ProgressBar progressBar)
        {
            var pricesList = new List<List<List<string>>>(urls.Length);

            await Task.WhenAll(urls.AsParallel().AsOrdered().Select(async url =>
            {
                pricesList.Add(await GetDayPricesAsync(url));
                Application.Current.Dispatcher.Invoke(() => progressBar.Value += 1);
            }));
            return pricesList;
        }

        public static async Task<List<List<string>>> GetDayPricesAsync(string url)
        {
            var config = Configuration.Default.WithDefaultLoader(); // конфиг для AngleSharp
            var document = await BrowsingContext.New(config).OpenAsync(url); // DOM исходник веб страницы
            
            var blocks = GetParse(in document, "tr", "js-rt-block-row "); // получаем блоки с категориями номеров и ценами
            List<List<string>> result = new List<List<string>>();
            foreach (var item in blocks)
            {
                List<string> _ = new List<string>();
                // парсим названия категорий
                var category = GetParse(in item, "span", "hprt-roomtype-icon-link ");
                foreach (var item_ in category)
                { 
                    _.Add(item_.TextContent.Trim()); 
                }
                // парсим цены
                var price = GetParse(in item, "div", "bui-price-display__value prco-inline-block-maker-helper prco-font16-helper ");
                foreach (var item_ in price)
                { 
                    _.Add(item_.TextContent.Trim()); 
                }
                result.Add(_);
            }
            return result;
        }

        private static IEnumerable<IElement> GetParse<T>(in T source, string htmlteg, string htmlclass) where T: IParentNode
        {
            IEnumerable<IElement> blocks = source.QuerySelectorAll(htmlteg).Where(item => item.ClassName != null && item.ClassName.Contains(htmlclass));
            return blocks;
        }
    }
}
