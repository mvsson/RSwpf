using System;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using RateShopperWPF.Models.InputModels;
using RateShopperWPF.Models.OutputModels;

namespace RateShopperWPF.Services.Core
{
    class ParsingService
    {   
        private readonly UrlsCreator UrlCreator;
        private readonly ParserCore Parser;
        /// <summary> Обрабатывает всплывающие уведомления. Первый параметр - текст, второй - заголовок</summary>
        private readonly Action<string, string> PopUpMessageHandler;

        #region Ctors
        public ParsingService(string inputLink)
        {
            UrlCreator = new UrlsCreator(inputLink);
            Parser = new ParserCore();
        }
        public ParsingService(string inputLink, Action<string, string> popUpMessageHandler) : this (inputLink)
        {
            PopUpMessageHandler += popUpMessageHandler;
        }
        #endregion 

        public async Task<double> GetMaxCountCategoriesAsync(ProgressBarModel loadingStatus)
        {
            PopUpMessageHandler?.Invoke("text ", "title");
            var urlPast6Month = UrlCreator.GetUrl(DateTime.Today.AddDays(180));
            var ratesPast6Month = await Parser.GetRatesListAsync(loadingStatus, urlPast6Month);
            double maxRatesCount = ratesPast6Month.First().Rates.Where(rate => rate.Category != null).Count();
            if (maxRatesCount == 0)
            {
                if (App.UserSettings.IsSoundOn)
                    SystemSounds.Exclamation.Play();
                _ = Task.Run(() => PopUpMessageHandler?.Invoke("Максимальное количество категорий не определено.\n" +
                                                           "Процентное соотношение не будет отображено.", "Error"));
            }
            return maxRatesCount;
        }

        public async Task<DateRates[]> GetRatesDataAsync(ProgressBarModel loadingStatus, DateTime[] Dates)
        {
            var urlsList = Dates.Select(date => UrlCreator.GetUrl(date)).ToArray();
            return await Parser.GetRatesListAsync(loadingStatus, urlsList);
        }
    }
}
