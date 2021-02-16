using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using RateShopperWPF.Models;

namespace RateShopperWPF.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {

        #region "Стартер"

        private DelegateCommand _starterCommand;
        public ICommand StarterCommand
        {
            get
            {
                if (_starterCommand == null)
                {
                    _starterCommand = new DelegateCommand(Starter_Click);
                }
                return _starterCommand;
            }
        }
        private async void Starter_Click()
        {
            // Выключаем UI
            IsEnabledStarterButton = false;
            IsEnabledDetailedCheckbox = false;

            UrlOnDate[] urls = UrlSettings.GetUrlsList(InputStartDate, InputEndDate, InputLink);
            UrlOnDate[][] splitUrls = UrlSettings.SplitUrlsListByN(urls, lenght: 16);

            LoadingStatus.Maximum = urls.Length+1;

            var urlPast6Month = UrlSettings.GetUrlsList(DateTime.Today.AddDays(180), DateTime.Today.AddDays(181), InputLink).First();
            var ratesPast6Month = await Parser.GetRatesListAsync(LoadingStatus, urlPast6Month);
            double maxRatesCount = ratesPast6Month.First().Rates.Where(rate => rate.Category != null).Count();

            IDataGridOutput printer = InputIsShowDetailed ? printer = new OutputDetailed() : new OutputShort();
            var chartLoader = new ChartLoader(InputLink);

            foreach (var _urls in splitUrls)
            {
                try // выгружаем инфу
                {
                    DateRates[] daysList = await Parser.GetRatesListAsync(LoadingStatus, _urls);

                    DateRates.GetGrid(printer, daysList).ToList().ForEach(item => GridSourse.Add(item));
                    
                    chartLoader.FillCharts(daysList, maxRatesCount);
                }
                catch (Exception ex)
                {
                    await Task.Run(() => MessageBox.Show(ex.Message));
                }
            }

            ChartMinRate.Add(chartLoader.ChartMinRate);
            ChartRatesCounter.Add(chartLoader.ChartRatesCounter);
            ChartRateCountPercent.Add(chartLoader.ChartRatesCounterPercent);
            chartLoader.Labels.ForEach(label => ChartLabels.Add(label));
            
            if (LoadingStatus.Value != LoadingStatus.Maximum)
                await Task.Run(() => MessageBox.Show("Таки где-то была ошибка в выгрузке данных, будь внимателен."));

            // включаем UI
            LoadingStatus.Value = 0;
            IsEnabledStarterButton = true;
            IsEnabledDetailedCheckbox = true;
        }
        #endregion

        private DelegateCommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new DelegateCommand(ClearCharts);
                }
                return _clearCommand;
            }
        }
        private void ClearCharts()
        {
            ChartRatesCounter.Clear();
            ChartRateCountPercent.Clear();
            ChartMinRate.Clear();
            ChartLabels.Clear();
            MessageBox.Show("Теперь чисто");
        }
        /*private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            //Use the axis MinValue/MaxValue properties to specify the values to display.
            //use double.Nan to clear it.

            X.MinValue = double.NaN;
            X.MaxValue = double.NaN;
            Y.MinValue = double.NaN;
            Y.MaxValue = double.NaN;
        }*/


    }
}
