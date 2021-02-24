using System;
using System.Windows;

namespace RateShopperWPF.Services.PopUpMessageService
{
    interface IPopUpMessageSender
    {
        void ShowMessage(string text, string caption);
        void ShowMessage(string text);
    }

    class PopUpMessageBoxSender : IPopUpMessageSender
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
