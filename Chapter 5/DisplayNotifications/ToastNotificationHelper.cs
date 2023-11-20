using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace DisplayNotifications
{
    public class ToastNotificationHelper
    {
        public static void ShowToastNotification(string title, string message)
        {
            // Construct the toast content
            ToastContent content = new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .GetToastContent();

            // Show the toast notification
            ToastNotificationManagerCompat.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
        }
    }
}
