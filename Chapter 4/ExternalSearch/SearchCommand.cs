using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Web;
using Task = System.Threading.Tasks.Task;

namespace ExternalSearch
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class SearchCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("20580f36-35da-4d5f-95d6-d2d2d4df35fd");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private SearchCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SearchCommand Instance
        {
            get;
            private set;
        }

        public static IVsOutputWindowPane OutputWindow
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public static DTE2 DteInstance
        {
            get;
            private set;
        }

        public static IVsStatusbar VsStatusBar
        {
            get;
            private set;
        }

        public static IVsUIShell VsUIShell
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in SearchCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            DteInstance = await package.GetServiceAsync(typeof(DTE)) as DTE2;
            Assumes.Present(DteInstance);
            OutputWindow = await package.GetServiceAsync(typeof(SVsGeneralOutputWindowPane)) as IVsOutputWindowPane;
            Assumes.Present(OutputWindow);
            VsStatusBar = await package.GetServiceAsync(typeof(IVsStatusbar)) as IVsStatusbar;
            Assumes.Present(VsStatusBar);
            VsUIShell = await package.GetServiceAsync(typeof(IVsUIShell)) as IVsUIShell;
            Assumes.Present(VsUIShell);
            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new SearchCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var options = package.GetDialogPage(typeof(ExternalSearchOptionPage)) as ExternalSearchOptionPage; // Get the options.

            if (!(DteInstance?.ActiveDocument?.Selection is TextSelection textSelection))
            {
                DteInstance.StatusBar.Text = "The selection is null or empty";
                return;
            }

            var textToBeSearched = textSelection.Text.Trim();
            if (string.IsNullOrWhiteSpace(textToBeSearched))
            {
                DteInstance.StatusBar.Text = "The selection is null or empty";
                return;
            }

            var encodedText = HttpUtility.UrlEncode(textToBeSearched);
            DteInstance.StatusBar.Text = $"Searching {textToBeSearched}";
            OutputWindow.OutputStringThreadSafe($"Searching {textToBeSearched}");
            var url = string.Format(options.Url, encodedText);
            if (options.UseVSBrowser)
            {
                DteInstance.ItemOperations.Navigate(url, vsNavigateOptions.vsNavigateOptionsDefault);
            }
            else
            {
                System.Diagnostics.Process.Start(url);
            }
        }

        public void ShowMessageBox(string title, string message)
        {
            Guid emptyGuid = Guid.Empty;           
            ThreadHelper.ThrowIfNotOnUIThread();
            VsUIShell.ShowMessageBox(
                dwCompRole: 0,
                rclsidComp: ref emptyGuid,
                pszTitle: title,
                pszText: message,
                pszHelpFile: null,
                dwHelpContextID: 0,
                msgbtn: OLEMSGBUTTON.OLEMSGBUTTON_OK,
                msgdefbtn: OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                msgicon: OLEMSGICON.OLEMSGICON_INFO,
                fSysAlert: 0,
                pnResult: out int result
            );
        }
    }
}
