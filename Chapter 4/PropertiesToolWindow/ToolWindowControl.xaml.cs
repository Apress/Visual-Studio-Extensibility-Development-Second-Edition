using System.Windows.Controls;

namespace PropertiesToolWindow
{
    /// <summary>
    /// Interaction logic for ToolWindowControl.
    /// </summary>
    public partial class ToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolWindowControl"/> class.
        /// </summary>
        public ToolWindowControl()
        {
            this.InitializeComponent();
        }

        public ToolWindowControl(ToolWindowData data) : this()
        {
            this.propertyGrid.SelectedObject = data;
        }
    }
}