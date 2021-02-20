using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RateShopperWPF.ViewModels.MainVM
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
        public DateTime InputStartDate {  get; set; }
        public DateTime InputEndDate {  get; set; }
        public bool InputIsShowDetailed { private get; set; }
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
