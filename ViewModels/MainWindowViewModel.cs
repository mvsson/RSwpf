using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts;
using RateShopperWPF.Infrastructure.Commands;
using RateShopperWPF.Models.OutputModels;
using RateShopperWPF.Services.Core;
using RateShopperWPF.Services.VMLoaders;
using RateShopperWPF.ViewModels.Base;
using LiveCharts.Configurations;

namespace RateShopperWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            #region Commands

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            ClearChartsCommand = new LambdaCommand(OnClearChartsCommandExecuted, CanClearChartsCommandExecute);

            GetDataOnBoardCommand = new LambdaCommand(OnGetDataOnBoardCommandExecuted, CanGetDataOnBoardCommandExecute);
            #endregion

            var config = Mappers.Xy<DateModel>()
                .X(dateModel => dateModel.Date.Ticks / TimeSpan.FromDays(1).Ticks)
                .Y(dateModel => dateModel.Value);

            ChartMinRate = new SeriesCollection(config);
            ChartRatesCounter = new SeriesCollection(config);
            ChartRateCountPercent = new SeriesCollection(config);
            GridSourse = new ObservableCollection<GridRateRow>();
            LoadingStatus = new ProgressBar();
        }

        #region "Commands"

        #region CloseAppCommand
        public ICommand CloseApplicationCommand { get; }
        private bool CanCloseApplicationCommandExecute(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region "Chart Clear"
        public ICommand ClearChartsCommand { get; }
        private bool CanClearChartsCommandExecute(object p) => true;
        private void OnClearChartsCommandExecuted(object p)
        {
            ChartRatesCounter.Clear();
            ChartRateCountPercent.Clear();
            ChartMinRate.Clear();
            if (App.UserSettings.IsSoundOn)
                SystemSounds.Exclamation.Play();
        }

        #endregion

        #region "GetDataOnBoard"

        public ICommand GetDataOnBoardCommand { get; }
        private bool CanGetDataOnBoardCommandExecute(object p) => true;
        private async void OnGetDataOnBoardCommandExecuted(object p)
        {
            // Выключаем UI
            IsEnabledStarterButton = false;
            IsEnabledDetailedCheckbox = false;
            LoadingStatus.Maximum = (InputEndDate - InputStartDate).TotalDays + 1;
            bool IsSoundOn = App.UserSettings.IsSoundOn;
            
            string inputLink = InputLink.Trim();


            DateTime[][] parsingDates = (new DatesCreator(InputStartDate, InputEndDate)).GetSplitDateList();
            var parser = new ParserWorker(inputLink);

            double maxCount = await parser.GetMaxCountCategoriesAsync(LoadingStatus);

            var chartLoader = new ChartLoader(inputLink, App.UserSettings.IsShowChartLabels);

            IGridLoader printer = InputIsShowDetailed ? printer = new GridLoaderDetailed() : new GridLoaderShort();
            var gridLoader = new GridLoader(printer);

            foreach (var dates in parsingDates)
            {
                try 
                {
                    var data = await parser.GetRatesDataAsync(LoadingStatus, dates);
                    gridLoader.GetGrid(data).ToList().ForEach(item => GridSourse.Add(item));
                    chartLoader.FillCharts(data, maxCount);
                }
                catch (Exception ex)
                {
                    if (IsSoundOn)
                        SystemSounds.Exclamation.Play();
                    _ = Task.Run(() => MessageBox.Show(ex.Message));
                }
            }
            ChartMinRate.Add(chartLoader.Charts.ChartMinRate);
            ChartRatesCounter.Add(chartLoader.Charts.ChartRatesCounter);
            ChartRateCountPercent.Add(chartLoader.Charts.ChartRatesCounterPercent);


            if (LoadingStatus.Value != LoadingStatus.Maximum)
                _ = Task.Run(() => MessageBox.Show("Таки где-то была ошибка в выгрузке данных, будь внимателен."));

            if (IsSoundOn)
                SystemSounds.Hand.Play();
            // включаем UI
            LoadingStatus.Value = 0;
            IsEnabledStarterButton = true;
            IsEnabledDetailedCheckbox = true;

        }
        #endregion

        #endregion


        #region "Charts Properties"
        private SeriesCollection _minRate;
        public SeriesCollection ChartMinRate
        {
            get => _minRate;
            set => Set(ref _minRate, value );
        }
        private SeriesCollection _ratesCounter;
        public SeriesCollection ChartRatesCounter
        {
            get => _ratesCounter;
            set => Set(ref _ratesCounter, value);
        }
        private SeriesCollection _ratePercent;
        public SeriesCollection ChartRateCountPercent
        {
            get => _ratePercent;
            set => Set(ref _ratePercent, value);
        }
        public Func<double, string> FormatterCounter { get; } = value => value + " кат.";
        public Func<double, string> FormatterCost { get; } = value => value + " руб.";
        public Func<double, string> FormatterPercent { get; } = value => value + " %";
        private readonly Func<double, string> _formatterX = value => value < 0.0 ?
                         new DateTime(0).ToString("d") :
                         new DateTime((long)(value * TimeSpan.FromDays(1).Ticks)).ToString("d");
        public Func<double, string> FormatterX { get => _formatterX; }

        #endregion

        #region "Grid Properties"

        private ObservableCollection<GridRateRow> _gridSourse;
        public ObservableCollection<GridRateRow> GridSourse
        {
            get => _gridSourse;
            set => Set(ref _gridSourse, value);
        }
        #endregion

        #region "Input Properties"

        public string InputLink { get; set; } = "ra-nevskiy-44.ru";
        public DateTime InputStartDate { private get; set; }
        public DateTime InputEndDate { private get; set; }
        public bool InputIsShowDetailed { private get; set; }
        #endregion

        #region "UI"

        private bool _isEnabledStarterButton = true;
        public bool IsEnabledStarterButton
        {
            get => _isEnabledStarterButton;
            set => Set(ref _isEnabledStarterButton, value);
        }
        private bool _isEnabledDetailedCheckbox = true;
        public bool IsEnabledDetailedCheckbox
        {
            get => _isEnabledDetailedCheckbox;
            set => Set(ref _isEnabledDetailedCheckbox, value);
        }
        private ProgressBar _loadingStatus;
        public ProgressBar LoadingStatus
        {
            get => _loadingStatus;
            set => Set(ref _loadingStatus, value);
        }
        #endregion
    }
}
