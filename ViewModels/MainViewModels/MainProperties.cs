using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using RateShopperWPF.Models;

namespace RateShopperWPF.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region "Input Properties"

        public string InputLink { get; set; } = "ra-nevskiy-44.ru";
        public DateTime InputStartDate { private get; set; } = DateTime.Today;
        public DateTime InputEndDate { private get; set; } = DateTime.Today.AddDays(1);
        public bool InputIsShowDetailed { private get; set; }
        #endregion


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


        #region "Enabled UI"

        private bool _isEnabledStarterButton = true;
        public bool IsEnabledStarterButton
        {
            get => _isEnabledStarterButton;
            set
            {
                _isEnabledStarterButton = value;
                OnPropertyChanged(nameof(IsEnabledStarterButton));
            }
        }
        private bool _isEnabledDetailedCheckbox = true;
        public bool IsEnabledDetailedCheckbox
        {
            get => _isEnabledDetailedCheckbox;
            set
            {
                _isEnabledDetailedCheckbox = value;
                OnPropertyChanged(nameof(IsEnabledDetailedCheckbox));
            }
        }
        #endregion
    }
}
