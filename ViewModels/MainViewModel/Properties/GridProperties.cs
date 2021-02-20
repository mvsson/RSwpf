using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using RateShopperWPF.Models.OutputModels;

namespace RateShopperWPF.ViewModels.MainVM
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        #region "Output Properties"

        private static ObservableCollection<GridRateRow> _gridSourse = new ObservableCollection<GridRateRow>();
        public ObservableCollection<GridRateRow> GridSourse
        {
            get => _gridSourse;
            set
            {
                _gridSourse = value;
                OnPropertyChanged(nameof(GridSourse));
            }
        }

        private ProgressBar _pb = new ProgressBar();
        public ProgressBar LoadingStatus
        {
            get => _pb;
            set
            {
                _pb = value;
                OnPropertyChanged(nameof(LoadingStatus));
            }
        }
        #endregion


    }
}
