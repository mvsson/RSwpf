using System;
using System.Windows;

namespace RateShopperWPF.Services.PopUpMessageService
{
    interface IPopUpMessageService
    {
        void ShowMessage(string text, string caption);
        void ShowMessage(string text);
    }

    class PopUpMessageBox : IPopUpMessageService
    {
        public void ShowMessage(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void ShowMessage(string text)
        {
            MessageBox.Show(text);
        }
    }
}
