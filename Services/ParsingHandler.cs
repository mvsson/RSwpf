﻿using System;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using RateShopperWPF.Models.OutputModels;
using RateShopperWPF.Services.Core;
using RateShopperWPF.Services.OutputLogic;

namespace RateShopperWPF.Services
{
    class ParsingHandler
    {
        #region InputProp
        private readonly DateTime StartDate;
        private readonly DateTime EndDate;
        private string _parentLink;
        public string ParentLink
        {
            private get => _parentLink;
            set
            {
                _parentLink = value;
                Charts = new ChartsModel(value, App.UserSettings.IsShowChartLabels);
                GridRows = new List<GridRowModel>();
            }
        }
        #endregion

        #region OutputProp
        public List<GridRowModel> GridRows { get; private set; }
        public ChartsModel Charts { get; private set; }
        #endregion

        public ParsingHandler(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public async Task ProcessAsync(ProgressBarModel progressBar)
        {
            DateTime[][] parsingDates = (new DatesCreator(StartDate, EndDate)).GetSplitDateList();
            var parser = new ParsingService(ParentLink);

            double maxCountCategory = await parser.GetMaxCountCategoriesAsync(progressBar);

            var chartLoader = new ChartLoader(maxCountCategory);
            var gridLoader = new GridLoader(App.UserSettings.IsShowGridDetailed);

            foreach (var dates in parsingDates)
            {
                try
                {
                    var data = await parser.GetRatesDataAsync(progressBar, dates);
                    GridRows.AddRange(gridLoader.GetRows(data));
                    foreach(var dayData in data)
                    {
                        Charts.ChartMinRate.Values.Add(chartLoader.AddPointMinRate(dayData));
                        Charts.ChartRatesCounter.Values.Add(chartLoader.AddPointRatesCounter(dayData));
                        Charts.ChartRatesCounterPercent.Values.Add(chartLoader.AddPointRatesCountPercent(dayData));
                    }
                }
                catch (Exception ex)
                {
                    if (App.UserSettings.IsSoundOn)
                        SystemSounds.Exclamation.Play();
                    _ = Task.Run(() => MessageBox.Show(ex.Message));
                }
            }
        }
    }
}
