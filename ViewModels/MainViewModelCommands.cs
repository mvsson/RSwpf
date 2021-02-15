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

            LoadingStatus.Maximum = urls.Length;

            IOutput printer = InputIsShowDetailed ? printer = new OutputDetailed() : new OutputShort();

            foreach (var _urls in splitUrls)
            {
                try // выгружаем инфу
                {
                    RatesByDay[] daysList = await Parser.GetRatesListAsync(LoadingStatus, _urls);

                    TextSource += RatesByDay.GetText(printer, daysList);

                    RatesByDay.GetGrid(printer, daysList).ToList().ForEach(item => GridSourse.Add(item));
                }
                catch (Exception ex)
                {
                    await Task.Run(() => MessageBox.Show(ex.Message));
                }
            }
            if (LoadingStatus.Value != LoadingStatus.Maximum)
                TextSource += "Таки где-то была ошибка в выгрузке данных, будь внимателен.";

            // включаем UI
            LoadingStatus.Value = 0;
            IsEnabledStarterButton = true;
            IsEnabledDetailedCheckbox = true;
        }

    }
}
