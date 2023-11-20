using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = ConfigureAwaitAnalyzer.Test.CSharpCodeFixVerifier<
    ConfigureAwaitAnalyzer.ConfigureAwaitAnalyzer,
    ConfigureAwaitAnalyzer.ConfigureAwaitAnalyzerCodeFixProvider>;

namespace ConfigureAwaitAnalyzer.Test
{
    [TestClass]
    public class ConfigureAwaitAnalyzerUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class Program
        {
            static async Task Main()
            {
               [|await Task.Delay(100);|]
            }
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class Program
        {
            static async Task Main()
            {
                await Task.Delay(100).ConfigureAwait(false);
            }
        }
    }";
            await VerifyCS.VerifyAnalyzerAsync(test);
            await VerifyCS.VerifyCodeFixAsync(test, fixtest);
        }
    }
}
