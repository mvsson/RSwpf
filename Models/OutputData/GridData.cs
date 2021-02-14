using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace RateShopperWPF.Models
{
    /// <summary>
    /// Класс для хранения и взаимодействия с ObservableCollection из ViewModel
    /// </summary>
    public class GridCollection//: INotifyPropertyChanged
    {
        /*public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/

        public ObservableCollection<DataGridRateRow> Source { get; set; }
        public GridCollection()
        {
            Source = new ObservableCollection<DataGridRateRow>();
        }
        public void AddRange(IList<DataGridRateRow> source)
        {
            source.ToList().ForEach(item => Source.Add(item));
            //OnPropertyChanged("GridSourse");

        }
        public void Add(DataGridRateRow item)
        {
            Source.Add(item);
            //OnPropertyChanged("GridSourse");
        }
    }
    /// <summary>
    /// Хранит в себе значения для вывода рядов в таблицу View
    /// </summary>
    public struct DataGridRateRow
    {
        public string Date { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string Meal { get; set; }
        public string ParentLink { get; set; }
        public DataGridRateRow(DateTime date, RateLine priceLine, string link)
        {
            Date = date.ToString("yyyy.MM.dd");
            Price = priceLine.Price;
            Category = priceLine.Category;
            Meal = priceLine.Meal;
            ParentLink = link;
        }
    }
}
