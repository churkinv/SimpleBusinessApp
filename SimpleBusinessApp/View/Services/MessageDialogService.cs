using System.Windows;

namespace SimpleBusinessApp.View.Services
{
    public enum MessageDialogResult
    {
        Ok,
        Cancel
    }

    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK
                ? MessageDialogResult.Ok
                : MessageDialogResult.Cancel;
        }
    }
}
