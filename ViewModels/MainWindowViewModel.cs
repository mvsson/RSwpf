using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LiveCharts;
using RateShopperWPF.Infrastructure.Commands;
using RateShopperWPF.Models.OutputModels;
using RateShopperWPF.ViewModels.Base;
using LiveCharts.Configurations;
using System.Collections.Generic;
using RateShopperWPF.Services;

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

            var config = Mappers.Xy<PointModel>()
                .X(dateModel => dateModel.Date.Ticks / TimeSpan.FromDays(1).Ticks)
                .Y(dateModel => dateModel.Value);

            ChartMinRate = new SeriesCollection(config);
            ChartRatesCounter = new SeriesCollection(config);
            ChartRateCountPercent = new SeriesCollection(config);
            GridSourse = new ObservableCollection<GridRowModel>();
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
            IsEnabledStarterButton = false;

            List<string> linkList = new List<string>();
            if (App.UserSettings.IsUseList)
                linkList.AddRange(App.UserSettings.ListLink.Where(item => item.IsSelected).Select(item => item.HotelLink));
            else
                linkList.Add(ParentLink.Trim());

            LoadingStatus = new ProgressBarModel((int)((EndDate - StartDate).TotalDays + 1)*linkList.Count());
            var handlerParser = new ParsingHandler(StartDate, EndDate);
            foreach(var inputLink in linkList)
            {
                handlerParser.ParentLink = inputLink;
                await handlerParser.ProcessAsync(LoadingStatus);

                handlerParser.GridRows.ForEach(row => GridSourse.Add(row));
                ChartMinRate.Add(handlerParser.Charts.ChartMinRate);
                ChartRatesCounter.Add(handlerParser.Charts.ChartRatesCounter);
                ChartRateCountPercent.Add(handlerParser.Charts.ChartRatesCounterPercent);
            }
            if (LoadingStatus.Value != LoadingStatus.MaxValue)
                _ = Task.Run(() => MessageBox.Show("Таки где-то была ошибка в выгрузке данных, будь внимателен."));
            if (App.UserSettings.IsSoundOn)
                SystemSounds.Hand.Play();
            
            LoadingStatus.Value = 0;
            IsEnabledStarterButton = true;
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

        private ObservableCollection<GridRowModel> _gridSourse;
        public ObservableCollection<GridRowModel> GridSourse
        {
            get => _gridSourse;
            set => Set(ref _gridSourse, value);
        }
        #endregion

        #region "Input Properties"

        public string ParentLink { get; set; } = "ra-nevskiy-44.ru";
        public DateTime StartDate { private get; set; }
        public DateTime EndDate { private get; set; }
        #endregion

        #region "UI"

        private bool _isEnabledInputLink = true;
        public bool IsEnabledInputLink
        {
            get => _isEnabledInputLink;
            set => Set(ref _isEnabledInputLink, value);
        }

        private bool _isEnabledStarterButton = true;
        public bool IsEnabledStarterButton
        {
            get => _isEnabledStarterButton;
            set => Set(ref _isEnabledStarterButton, value);
        }

        private ProgressBarModel _loadingStatus;
        public ProgressBarModel LoadingStatus
        {
            get => _loadingStatus;
            set => Set(ref _loadingStatus, value);
        }
        #endregion
    }
}
