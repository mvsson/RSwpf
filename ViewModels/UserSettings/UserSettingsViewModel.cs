using System.Collections.ObjectModel;
using RateShopperWPF.Models.InputModels;
using RateShopperWPF.ViewModels.Base;

namespace RateShopperWPF.ViewModels.UserSettings
{
    public class UserSettingsViewModel : ViewModelBase
    {
        public UserSettingsViewModel()
        {
            IsUseList = false;
            IsShowChartLabels = true;
            IsSoundOn = true;
            IsShowGridDetailed = false;
            ListLink = new ObservableCollection<HotelLinkSetter>();
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
                IsEnabledInputLink = _isUseList == true ? false : true ;
            }
        }
        private bool _isEnabledInputLink;
        public bool IsEnabledInputLink
        {
            get => _isEnabledInputLink;
            set => Set(ref _isEnabledInputLink, value);
        }


        private bool _isSoundOn;
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
}
