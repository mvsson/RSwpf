using System;
using System.ComponentModel;
using System.Linq;
using System.Media;
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
                    _starterCommand = new DelegateCommand(StarterClick);
                }
                return _starterCommand;
            }
        }
        private async void StarterClick()
        {
            // Выключаем UI
            IsEnabledStarterButton = false;
            IsEnabledDetailedCheckbox = false;
            LoadingStatus.Maximum = (InputEndDate - InputStartDate).TotalDays + 1;

            UrlOnDate[] urls = UrlSettings.GetUrlsList(InputStartDate, InputEndDate, InputLink);
            UrlOnDate[][] splitUrls = UrlSettings.SplitUrlsListByN(urls, lenght: 16);

            // Извлекаем максимальное возможное количество доступных категорий
            var urlPast6Month = UrlSettings.GetUrlsList(DateTime.Today.AddDays(180), DateTime.Today.AddDays(181), InputLink).First();
            var ratesPast6Month = await Parser.GetRatesListAsync(LoadingStatus, urlPast6Month);
            double maxRatesCount = ratesPast6Month.First().Rates.Where(rate => rate.Category != null).Count();
            if (maxRatesCount == 0)
            {
                SystemSounds.Exclamation.Play();
                _ = Task.Run(() => MessageBox.Show("Максимальное количество категорий не определено.\n" +
                                                   "Процентное соотношение не будет отображено."));
            }
                

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
                    SystemSounds.Exclamation.Play();
                    _ = Task.Run(() => MessageBox.Show(ex.Message));
                }
            }

            // заполняем графики
            ChartMinRate.Add(chartLoader.ChartMinRate);
            ChartRatesCounter.Add(chartLoader.ChartRatesCounter);
            ChartRateCountPercent.Add(chartLoader.ChartRatesCounterPercent);

            if (LoadingStatus.Value != LoadingStatus.Maximum)
                _ = Task.Run(() => MessageBox.Show("Таки где-то была ошибка в выгрузке данных, будь внимателен."));

            SystemSounds.Hand.Play();
            // включаем UI
            LoadingStatus.Value = 0;
            IsEnabledStarterButton = true;
            IsEnabledDetailedCheckbox = true;
        }
        #endregion

        #region "Chart Commands"
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
            SystemSounds.Exclamation.Play();
            MessageBox.Show("НАЧАЛЬНИК, БЛЯДЬ! НАЧАЛЬНИК! ЭТОТ ПИДОРАС ОБОСРАЛСЯ, БЛЯ! НАЧАЛЬНИК! " +
                            "Иди под струю, блядь, чтоб ща пришли, ты чистый был нахуй! ТЫ ПОНЯЛ БЛЯАААДЬ! " +
                            "ЧТОБЫ ЧИСТЫЙ БЫЛ, СУКА! Обосрался, пидорас, а. НАЧАЛЬНИК, БЛЯДЬ, ЭТОТ ОБОСРАЛСЯ! " +
                            "ИДИТЕ МОЙТЕ ЕГО НАХУЙ, Я С НИМ ЗДЕСЬ СИДЕТЬ НЕ БУДУ, БЛЯДЬ!");
        }
        


        #endregion
    }
}
