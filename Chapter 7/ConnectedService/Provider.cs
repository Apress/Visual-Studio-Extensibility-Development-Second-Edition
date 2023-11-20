// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Microsoft.VisualStudio.ConnectedServices;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ConnectedService
{
    [ConnectedServiceProviderExport(Constants.ProviderId)]
    internal class Provider : ConnectedServiceProvider
    {
        public Provider()
        {
            Category = Constants.Category;
            Name = Constants.Name;
            Description = Constants.Description;
            Id = Constants.ProviderId;
            Icon = new BitmapImage() { UriSource = new Uri("ConnectedServices.png", UriKind.Relative) };
            Name = Constants.Name;
            SupportsUpdate = true;
            Version = Assembly.GetExecutingAssembly().GetName().Version;
            MoreInfoUri = new Uri("https://rishabhverma.net");
        }

        public override Task<ConnectedServiceConfigurator> CreateConfiguratorAsync(ConnectedServiceProviderContext context)
        {
            return Task.FromResult<ConnectedServiceConfigurator>(new Wizard(context));
        }
    }   
}
