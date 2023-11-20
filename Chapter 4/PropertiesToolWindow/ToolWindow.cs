using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Windows.Markup;

namespace PropertiesToolWindow
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid(ToolWindowId)]
    public class ToolWindow : ToolWindowPane
    {
        internal const string ToolWindowId = "a26b1099-d844-4461-9f4c-79f49c5b8257";

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindow"/> class.
        /// </summary>
        public ToolWindow(ToolWindowData data) : base()
        {
            this.Caption = "Automation Properties";
            this.BitmapImageMoniker = KnownMonikers.ListProperty;
            this.Content = new ToolWindowControl(data);
        }
    }
}
