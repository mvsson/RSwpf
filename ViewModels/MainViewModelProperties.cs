using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;
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

        private static GridCollection _gridSourse = new GridCollection();
        public ObservableCollection<DataGridRateRow> GridSourse
        {
            get
            {
                return _gridSourse.Source; 
            }
            set
            {
                _gridSourse.Source = value;
                OnPropertyChanged(nameof(GridSourse));
            }
        }
        private string _textSource;
        public string TextSource
        {
            get
            {
                return _textSource;
            }
            set
            {
                _textSource = value;
                OnPropertyChanged(nameof(TextSource));
            }
        }
        public string InputLink { get; set; } = "ra-nevskiy-44.ru";

        public DateTime InputStartDate { private get; set; } = DateTime.Today;
        public DateTime InputEndDate { private get; set; } = DateTime.Today.AddDays(1);
        public bool InputIsShowDetailed { private get; set; }
        private bool _isEnabledStarterButton = true;
        public bool IsEnabledStarterButton
        {
            get
            {
                return _isEnabledStarterButton;
            }
            set
            {
                _isEnabledStarterButton = value;
                OnPropertyChanged(nameof(IsEnabledStarterButton));
            }
        }
        private bool _isEnabledDetailedCheckbox = true;
        public bool IsEnabledDetailedCheckbox
        {
            get
            {
                return _isEnabledDetailedCheckbox;
            }
            set
            {
                _isEnabledDetailedCheckbox = value;
                OnPropertyChanged(nameof(IsEnabledDetailedCheckbox));
            }
        }
        private ProgressBar _pb = new ProgressBar();
        public ProgressBar PBprogressBar
        {
            get
            {
                return _pb;
            }
            set
            {
                _pb = value;
                //OnPropertyChanged(nameof(PBprogressBar));
            }
        }
    }
}
