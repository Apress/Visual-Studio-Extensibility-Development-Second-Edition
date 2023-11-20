// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Microsoft.VisualStudio.ConnectedServices;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectedService
{
    [ConnectedServiceHandlerExport(Constants.ProviderId, AppliesTo = "CSharp")]
    public class Handler : ConnectedServiceHandler
    {
        public override async Task<AddServiceInstanceResult> AddServiceInstanceAsync(ConnectedServiceHandlerContext context, CancellationToken ct)
        {
            var instance = (Instance)context.ServiceInstance;
            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, 
                $"Generating code to get universities of default country set as {instance.Country}");
            var csharpFilePath = await GenerateCSharpFileAsync(context, instance);
            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, $"Generated {Path.GetFileName(csharpFilePath)}");
            var folderName = context.ServiceInstance.Name;
            var result = new AddServiceInstanceResult(folderName, null);
            return result;
        }

        public override async Task<UpdateServiceInstanceResult> UpdateServiceInstanceAsync(ConnectedServiceHandlerContext context, 
            CancellationToken cancellationToken)
        {
            var instance = (Instance)context.ServiceInstance;
            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, 
                $"Re-generating code to get universities of default country set as {instance.Country}");
            var csharpFilePath = await GenerateCSharpFileAsync(context, instance);
            await context.Logger.WriteMessageAsync(LoggerMessageCategory.Information, $"Re-generated code based on {csharpFilePath}");
            return await base.UpdateServiceInstanceAsync(context, cancellationToken);
        }

        private async Task<string> GenerateCSharpFileAsync(ConnectedServiceHandlerContext context, Instance instance)
        {
            string country = string.IsNullOrWhiteSpace(instance.Country) ? "India" : instance.Country;
            var folderPath = Path.GetDirectoryName(context.ProjectHierarchy.GetProject().FullName);
            var fileName = $"RequestHelper.cs";
            var generatedFullPath = Path.Combine(folderPath, fileName);
            if (File.Exists(generatedFullPath))
            {
                File.Delete(generatedFullPath);
            }

            var namespaceName = context.ProjectHierarchy.GetProject().GetNameSpace();
            var template = Resource.ClientHelperTemplate;
            var updatedTemplate = template.Replace("##NAMESPACE##", namespaceName).Replace("##COUNTRY##", country);
            File.WriteAllText(generatedFullPath, updatedTemplate);
            await context.HandlerHelper.AddFileAsync(generatedFullPath, fileName);
            return generatedFullPath;
        }
    }
}
