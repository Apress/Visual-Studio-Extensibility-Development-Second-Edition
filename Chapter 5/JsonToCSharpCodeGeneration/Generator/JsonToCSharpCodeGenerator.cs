using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using NJsonSchema.CodeGeneration.CSharp;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JsonToCSharpCodeGeneration
{
    [Guid("f0ff9543-4996-4be8-9061-c57131998819")]
    public class JsonToCSharpCodeGenerator : BaseCodeGeneratorWithSite
    {
        public const string Name = nameof(JsonToCSharpCodeGenerator);

        public const string Description = "Generates the C# class from JSON file";

        public override string GetDefaultExtension() => ".cs";

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            string document = string.Empty;
            try
            {
                document = ThreadHelper.JoinableTaskFactory.Run(async () =>
               {
                   var text = File.ReadAllText(inputFileName); // Alternatively, you can also use inputFileContent directly.
                   var schema = NJsonSchema.JsonSchema.FromSampleJson(text);
                   var generator = new CSharpGenerator(schema);
                   return await Task.FromResult(generator.GenerateFile());
               });
            }
            catch (Exception ex)
            {
                LogException(ex);
            }

            return Encoding.UTF8.GetBytes(document);
        }

        private void LogException(Exception ex)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            // Write to output window.
            var outputWindowPane = GetService(typeof(SVsGeneralOutputWindowPane)) as IVsOutputWindowPane;
            outputWindowPane?.OutputStringThreadSafe($"An exception occurred while generating code {ex}.");

            // Show in error list.
            GeneratorErrorCallback(false, 1, $"An exception occurred while generating code {ex}.", 1, 1);
            ErrorList.ForceShowErrors();
            ErrorList.BringToFront();
        }
    }
}
