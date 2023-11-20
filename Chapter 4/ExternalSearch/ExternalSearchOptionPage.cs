using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace ExternalSearch
{
    [Guid("f6528658-87a9-4b49-bcc3-5647df1e68bd")]
    public sealed class ExternalSearchOptionPage : DialogPage
    {
        private const string DefaultUrl = "https://www.google.com/search?q={0}";
        private static readonly Dictionary<SearchEngines, string> allEngines = new Dictionary<SearchEngines, string>()
        {
            { SearchEngines.Google, DefaultUrl },
            { SearchEngines.Bing, "https://www.bing.com/search?q={0}" },
            { SearchEngines.MicrosoftLearn, "https://learn.microsoft.com/en-in/search/?terms={0}&category=All" },
            { SearchEngines.StackOverflow, "https://stackoverflow.com/search?q={0}" },
            { SearchEngines.GitHub, "https://github.com/search?q={0}" }
        };

        [DisplayName("Use Visual Studio Browser")]
        [DefaultValue(true)]
        [Category("General")]
        [Description("A value indicating whether search should be displayed in Visual Studio browser or an external browser")]
        public bool UseVSBrowser { get; set; }

        [DisplayName("Search Engine")]
        [DefaultValue("Google")]
        [Category("General")]
        [Description("The Search Engine to be used for searching")]
        [TypeConverter(typeof(EnumConverter))]
        public SearchEngines SearchEngine { get; set; } = SearchEngines.Google;

        [DisplayName("Url")]
        [Category("General")]
        [Description("The Search Engine url to be used for searching")]
        [Browsable(false)]
        public string Url
        {
            get
            {
                var selectedEngineUrl = allEngines.FirstOrDefault(j => j.Key == SearchEngine).Value;
                return string.IsNullOrWhiteSpace(selectedEngineUrl) ? DefaultUrl : selectedEngineUrl;
            }
        }
    }

    public enum SearchEngines
    {
        Google = 0,
        Bing,
        MicrosoftLearn,
        StackOverflow,
        GitHub
    }
}
