using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AngleSharp;
using AngleSharp.Dom;


namespace RateShopperWPF
{
    class PriceParser
    {
        public static void GetPricesList(string[] urls, ref ProgressBar progressBar, in bool showDetailed, 
            in UrlSettings hotelUrlSettings, in DateSettings parsingDates, ref TextBox output)
        {
            DateTime dateCount = parsingDates.Start;
            var pricesList = new List<List<List<string>>>(urls.Length);


            Task.WhenAll(urls.AsParallel().AsOrdered().Select(async url =>
            {
                pricesList.Add(await GetDayPricesAsync(url).ConfigureAwait(false));
            }));

            if (!showDetailed)
                output.Text += "Минимальные цены в отеле " + hotelUrlSettings.HotelLink + ", на даты:" + "\n";
            for (int i = 0; i < urls.Length; i++)  // перебираем даты
            {
                try
                {
                    if (showDetailed) // "показать подробно"
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
                progressBar.Value = i + 1;
            }
        }

        public static async Task<List<List<string>>> GetDayPricesAsync(string url)
        {
            var config = Configuration.Default.WithDefaultLoader(); // конфиг для AngleSharp
            var document = await BrowsingContext.New(config).OpenAsync(url).ConfigureAwait(false); // DOM исходник веб страницы
            
            var blocks = GetParse(in document, "tr", "js-rt-block-row "); // получаем блоки с категориями номеров и ценами
            List<List<string>> result = new List<List<string>>();
            foreach (var item in blocks)
            {
                List<string> _ = new List<string>();
                // парсим названия категорий
                var category = GetParse(in item, "span", "hprt-roomtype-icon-link ");
                foreach (var item_ in category)
                { _.Add(item_.TextContent.Trim()); }
                // парсим цены
                var price = GetParse(in item, "div", "bui-price-display__value prco-inline-block-maker-helper prco-font16-helper ");
                foreach (var item_ in price)
                { _.Add(item_.TextContent.Trim()); }
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
