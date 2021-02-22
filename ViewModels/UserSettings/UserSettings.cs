using System.Collections.ObjectModel;
using RateShopperWPF.ViewModels.Base;

namespace RateShopperWPF.ViewModels.UserSettings
{
    public class UserSettings : ViewModelBase
    {
        internal readonly MainWindowViewModel MainVM;
        public UserSettings()
        {
            ListLink = new ObservableCollection<HotelLinkSetter>();
            MainVM = new MainWindowViewModel();
        }

        #region "Global Settings"
        private bool _isShowGridDetailed;
        public bool IsShowGridDetailed
        {
            get => _isShowGridDetailed;
            set => Set(ref _isShowGridDetailed, value);
        }

        private bool _isShowChartLabels;
        public bool IsShowChartLabels
        {
            get => _isShowChartLabels;
            set => Set(ref _isShowChartLabels, value);
        }

        private bool _isUseList;
        public bool IsUseList
        {
            get => _isUseList;
            set
            {
                Set(ref _isUseList, value);
                MainVM.IsEnabledInputLink = !value;
                OnPropertyChanged("IsEnabledInputLink");
            }
        }

        private bool _isSoundOn = true;
        public bool IsSoundOn
        {
            get => _isSoundOn;
            set => Set(ref _isSoundOn, value);
        }

        private ObservableCollection<HotelLinkSetter> _listLink;
        public ObservableCollection<HotelLinkSetter> ListLink
        {
            get => _listLink;
            set => Set(ref _listLink, value);
        }
        #endregion
    }

    public class HotelLinkSetter  //модель
    {
        public bool IsSelected { get; set; } = true;
        public string HotelLink { get; set; }
    }
}
