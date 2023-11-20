using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AICompanion
{
    internal sealed class Helper
    {
        private static string s_apiKey = Environment.GetEnvironmentVariable("OpenAIAPIKey");

        internal static IVsOutputWindowPane OutputWindow
        {
            get;
            private set;
        }

        internal static DTE2 DteInstance
        {
            get;
            private set;
        }

        internal static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            DteInstance = DteInstance ?? await package.GetServiceAsync(typeof(DTE)) as DTE2;
            Assumes.Present(DteInstance);
            OutputWindow = OutputWindow ?? await package.GetServiceAsync(typeof(SVsGeneralOutputWindowPane)) as IVsOutputWindowPane;
            Assumes.Present(OutputWindow);
            OutputWindow.SetName("ChatGPT AI Assistant");
        }

        internal static async Task ReviewSelectedCodeAsync()
        {
            var prompt = "Review the code and suggest improvements";
            await AskChatGptAsync(prompt);
        }

        internal static async Task ExplainSelectedCodeAsync()
        {
            var prompt = "Explain the code in detail";
            await AskChatGptAsync(prompt);
        }

        internal static async Task OptimizeSelectedCodeAsync()
        {
            var prompt = "Optimize the code in detail";
            await AskChatGptAsync(prompt);
        }

        private static async Task AskChatGptAsync(string prompt)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            var selectedText = GetSelectedText();
            if (string.IsNullOrWhiteSpace(selectedText))
            {
                DteInstance.StatusBar.Text = "The selection is null or empty.";
                return;
            }
            
            var output = await AskChatGptAsync(prompt, selectedText);
            ShowInOutputWindow(output);
        }

        private static string GetSelectedText()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!(DteInstance?.ActiveDocument?.Selection is TextSelection textSelection))
            {
                DteInstance.StatusBar.Text = "The selection is null or empty.";
                return string.Empty;
            }

            return textSelection.Text?.Trim();
        }

        private static async Task<string> AskChatGptAsync(string prompt, string selectedText)
        {
            var openai = new OpenAIAPI(s_apiKey);
            var query = $"{prompt} {selectedText}";
            CompletionRequest completionRequest = new CompletionRequest
            {
                Prompt = query,
                Model = OpenAI_API.Models.Model.DavinciText,
                MaxTokens = 1024
            };

            var completions = await openai.Completions.CreateCompletionAsync(completionRequest);
            StringBuilder result = new StringBuilder();
            foreach (var completion in completions.Completions)
            {
                result.Append(completion.Text);
            }

            return result.ToString();
        }

        private static void ShowInOutputWindow(string message)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            OutputWindow.OutputStringThreadSafe(message);
            OutputWindow.OutputStringThreadSafe(Environment.NewLine + "---------------------------------");
            OutputWindow.Activate();
        }
    }
}
