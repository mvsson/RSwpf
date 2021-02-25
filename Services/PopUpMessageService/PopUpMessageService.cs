using System;
using System.Windows;

namespace RSwpf.Services.PopUpMessageService
{
    class PopUpMessageArgs : EventArgs
    {
        public readonly string Message;
        public readonly string Title;
        public PopUpMessageArgs(string message, string title)
        {
            Message = message;
            Title = title;
        }
    }

    interface IPopUpMessageSender
    {
        void ShowMessage(object sender, PopUpMessageArgs args);
        void ShowMessage(string text);
    }

    class PopUpMessageBoxSender : IPopUpMessageSender
    {
        public void ShowMessage(object sender, PopUpMessageArgs args)
        {
            MessageBox.Show(args.Message, args.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void ShowMessage(string text)
        {
            MessageBox.Show(text);
        }
    }
}
