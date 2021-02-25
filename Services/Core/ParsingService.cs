using System;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using RSwpf.Models.InputModels;
using RSwpf.Models.OutputModels;
using RSwpf.Services.PopUpMessageService;

namespace RSwpf.Services.Core
{
    class ParsingService
    {   
        private readonly UrlCreator UrlCreator;
        private readonly ParserCore Parser;
        /// <summary> Обрабатывает всплывающие уведомления. Первый параметр - текст, второй - заголовок</summary>
        private readonly Action<object, PopUpMessageArgs> PopUpMessageHandler;

        #region Ctors
        public ParsingService(string inputLink)
        {
            UrlCreator = new UrlCreator(inputLink);
            Parser = new ParserCore();
        }
        public ParsingService(string inputLink, Action<object, PopUpMessageArgs> popUpMessageHandler) : this (inputLink)
        {
            PopUpMessageHandler += popUpMessageHandler;
        }
        #endregion 

        public async Task<double> GetMaxCountCategoriesAsync(ProgressBarModel loadingStatus)
        {
            var urlPast6Month = UrlCreator.GetUrl(DateTime.Today.AddDays(180));
            var ratesPast6Month = await Parser.GetRatesOnDateAsync(loadingStatus, urlPast6Month);
            double maxRatesCount = ratesPast6Month.Rates.Where(rate => rate.Category != null).Count();
            if (maxRatesCount == 0)
            {
                if (App.UserSettings.IsSoundOn)
                    SystemSounds.Exclamation.Play();
                _ = Task.Run(() => PopUpMessageHandler?.Invoke(this, new PopUpMessageArgs("Максимальное количество категорий не определено.\n" +
                                                           "Процентное соотношение не будет отображено.", "Error")));
            }
            return maxRatesCount;
        }

        public async Task<DateRates[]> GetRatesOnDatesAsync(ProgressBarModel loadingStatus, DateTime[] Dates)
        {
            var urlsList = Dates.Select(date => UrlCreator.GetUrl(date)).ToArray();
            return await Parser.GetRatesOnDatesAsync(loadingStatus, urlsList);
        }
    }
}
