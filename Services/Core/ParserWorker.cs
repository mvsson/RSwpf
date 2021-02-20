﻿using System;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RateShopperWPF.Models.InputModels;

namespace RateShopperWPF.Services.Core
{
    class ParserWorker
    {   
        private readonly UrlsCreator UrlCreator;
        private readonly ParserCore Parser;

        public ParserWorker(string inputLink)
        {
            UrlCreator = new UrlsCreator(inputLink);
            Parser = new ParserCore();
        }
    
        public async Task<double> GetMaxCountCategoriesAsync(ProgressBar loadingStatus)
        {
            var urlPast6Month = UrlCreator.GetUrl(DateTime.Today.AddDays(180));
            var ratesPast6Month = await Parser.GetRatesListAsync(loadingStatus, urlPast6Month);
            double maxRatesCount = ratesPast6Month.First().Rates.Where(rate => rate.Category != null).Count();
            if (maxRatesCount == 0)
            {
                if (App.UserSettings.IsSoundOn)
                    SystemSounds.Exclamation.Play();
                _ = Task.Run(() => MessageBox.Show("Максимальное количество категорий не определено.\n" +
                                                           "Процентное соотношение не будет отображено."));
            }
            return maxRatesCount;
        }

        public async Task<DateRates[]> GetRatesDataAsync(ProgressBar loadingStatus, DateTime[] Dates)
        {
            var urlsList = Dates.Select(date => UrlCreator.GetUrl(date)).ToArray();
            return await Parser.GetRatesListAsync(loadingStatus, urlsList);
        }
    }
}
