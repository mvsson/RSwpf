using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RSwpf.Models.OutputModels
{
    class ProgressBarModel : INotifyPropertyChanged
    {
        private int _value;
        public int Value
        {
            get => _value;
            set => Set(ref _value, value);
        }
        private int _maxValue;
        public int MaxValue
        {
            get => _maxValue;
            set => Set(ref _maxValue, value);
        }

        public ProgressBarModel(int maxValue)
        {
            _value = 0;
            MaxValue = maxValue;
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
        #endregion
    }
}
