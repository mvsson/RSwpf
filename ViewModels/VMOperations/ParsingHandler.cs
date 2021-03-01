using System;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;
using RSwpf.Models.OutputModels;
using RSwpf.Services.Core;
using RSwpf.Services.PopUpMessageService;

namespace RSwpf.ViewModels.VMOperations
{
    /// <summary>
    /// Инкапсулирует в себе весь алгоритм парсинга. Содержит свойства GridRows и Charts для вывода информации в таблицу и графики.
    /// </summary>
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

        /// <summary> Обрабатывает всплывающие уведомления. Первый параметр - текст, второй - заголовок</summary>
        private readonly Action<object, PopUpMessageArgs> PopUpMessageHandler;
        #endregion

        #region Ctors
        public ParsingHandler(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
        public ParsingHandler(DateTime startDate, DateTime endDate, Action<object, PopUpMessageArgs> popUpMessageHandler) : this(startDate, endDate)
        {
            PopUpMessageHandler += popUpMessageHandler;
        }
        #endregion

        public async Task GoParseAsync(ProgressBarModel progressBar)
        {
            DateTime[][] parsingDates = (new DatesCreator(StartDate, EndDate)).GetSplitDates();
            var parser = new ParsingService(ParentLink, PopUpMessageHandler);

            double maxCountCategory = await parser.GetMaxCountCategoriesAsync(progressBar);

            var chartLoader = new ChartLoader(maxCountCategory);
            var gridLoader = new GridLoader(App.UserSettings.IsShowGridDetailed);

            foreach (var dates in parsingDates)
            {
                try
                {
                    var data = await parser.GetRatesOnDatesAsync(progressBar, dates);
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
                    _ = Task.Run(() => PopUpMessageHandler?.Invoke(this, new PopUpMessageArgs(ex.Message, ex.Source)));
                }
            }
        }
    }
}
