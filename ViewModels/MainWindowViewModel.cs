using System;
using System.Collections.ObjectModel;
using System.Media;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using RateShopperWPF.Infrastructure.Commands;
using RateShopperWPF.Models.OutputModels;
using RateShopperWPF.ViewModels.Base;
using LiveCharts.Configurations;
using System.Collections.Generic;
using RateShopperWPF.Services;
using RateShopperWPF.Services.PopUpMessageService;

namespace RateShopperWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Ctor
        public MainWindowViewModel()
        {
            #region Commands

            ClearChartsCommand = new LambdaCommand(OnClearChartsCommandExecuted, CanClearChartsCommandExecute);

            GetDataOnBoardCommand = new AsyncCommand<object>(OnGetDataOnBoardCommandExecuted, CanGetDataOnBoardCommandExecute);
            #endregion

            #region Initialize prop
            var config = Mappers.Xy<PointModel>()
                .X(dateModel => dateModel.Date.Ticks / TimeSpan.FromDays(1).Ticks)
                .Y(dateModel => dateModel.Value);            
            ChartMinRate = new SeriesCollection(config);
            ChartRatesCounter = new SeriesCollection(config);
            ChartRateCountPercent = new SeriesCollection(config);
            GridSourse = new ObservableCollection<GridRowModel>();
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(14);
            #endregion
        }
        public MainWindowViewModel(IPopUpMessageSender popUpService) : this ()
        {
            PopUpSender = popUpService;
        }
        #endregion

        private readonly IPopUpMessageSender PopUpSender;

        #region "Commands"

        #region "Chart Clear Command"
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

        #region "Get Data On Board AsyncCommand"

        #region "Is Busy Async Context"
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set => Set(ref _isBusy, value);
        }
        #endregion
        public ICommand GetDataOnBoardCommand { get; }
        private bool CanGetDataOnBoardCommandExecute(object p) => !IsBusy;
        private async Task OnGetDataOnBoardCommandExecuted(object p)
        {
            try
            {
                IsBusy = true;
                IsEnabledStarterButton = false;
                List<string> parsingLinks = App.UserSettings.IsUseList ? GetParsingLinksFromSettings() : GetParsingLinkFromMainWindow();

                LoadingStatus = new ProgressBarModel((int)((EndDate - StartDate).TotalDays + 1) * parsingLinks.Count());
                var handlerParser = new ParsingHandler(StartDate, EndDate, PopUpSender);

                foreach (var link in parsingLinks)
                {
                    handlerParser.ParentLink = link;
                    await handlerParser.ProcessAsync(LoadingStatus);

                    handlerParser.GridRows.ForEach(row => GridSourse.Add(row));
                    ChartMinRate.Add(handlerParser.Charts.ChartMinRate);
                    ChartRatesCounter.Add(handlerParser.Charts.ChartRatesCounter);
                    ChartRateCountPercent.Add(handlerParser.Charts.ChartRatesCounterPercent);
                }
                if (LoadingStatus.Value != LoadingStatus.MaxValue)
                    _ = Task.Run(() => PopUpSender?.ShowMessage("Таки где-то была ошибка в выгрузке данных, будь бдителен."));
                if (App.UserSettings.IsSoundOn)
                    SystemSounds.Hand.Play();
            }
            catch (Exception ex)
            {
                PopUpSender?.ShowMessage(ex.Message, ex.Source);
            }
            finally
            {
                LoadingStatus.Value = 0;
                IsEnabledStarterButton = true;
                IsBusy = false;
            }
        }
        #region "GetParsingLinks Method"
        private List<string> GetParsingLinksFromSettings()
        {
            return (App.UserSettings.ListLink.Where(item => item.IsSelected)
                            .Select(item => item.HotelLink.Trim())).ToList();
        }
        private List<string> GetParsingLinkFromMainWindow()
        {
            return new List<string> { ParentLink.Trim() };
        }
        #endregion
        #endregion "Get Data On Board Command"

        #endregion "Commands"


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
        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (value < DateTime.Today)
                    Set(ref _startDate, DateTime.Today);
                else
                    Set(ref _startDate, value);

                if (EndDate <= StartDate)
                    EndDate = value.AddDays(1);
            }
        }
        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (value <= StartDate)
                    Set(ref _endDate, StartDate.AddDays(1));
                else
                    Set(ref _endDate, value);
            }
        }
        #endregion

        #region "UI"

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
