using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.Windows.Controls;

namespace PropertiesToolWindow
{
    [DisplayName("Tool Window Data")]
    public class ToolWindowData
    {
        [DisplayName("DTE Instance")]
        [Category("General")]
        [Description("The DTE Instance")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DTE2 DTE { get; set; }

        [DisplayName("Async Package")]
        [Category("General")]
        [Description("The Package")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AsyncPackage Package { get; set; }

        [DisplayName("Text Box")]
        [Category("General")]
        [Description("The TextBox")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TextBox TextBox { get; set; }
    }
}
