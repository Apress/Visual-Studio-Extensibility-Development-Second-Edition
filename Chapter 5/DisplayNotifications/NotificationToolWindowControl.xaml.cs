using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace DisplayNotifications
{
    /// <summary>
    /// Interaction logic for NotificationToolWindowControl.
    /// </summary>
    public partial class NotificationToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationToolWindowControl"/> class.
        /// </summary>
        public NotificationToolWindowControl()
        {
            this.InitializeComponent();
            // Define the text to be displayed in Infobar.
            var text = "Welcome to Chapter 5. Are you liking it?";
            // Show in main window infobar host.
            InfoBarService.Instance.ShowInfoBar(text);
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            // Infobar.
            InfoBarService.Instance.ShowInfoBar($"This info bar is invoked from tool window button. Are you liking it?", NotificationToolWindowCommand.ToolWindow);

            // StatusBar
            
            NotificationToolWindowCommand.DteInstance.StatusBar.Text = "This is a demo of notification in statusbar.";
            NotificationToolWindowCommand.VsStatusbar.SetText("This is a demo of notification in statusbar.");

            // Toast
            ToastNotificationHelper.ShowToastNotification("Toast", "This is a sample of toast notification.");

            // DialogBox
            System.Windows.MessageBox.Show("This is the demo of message in dialog box", "Visual Studio Dialog", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}