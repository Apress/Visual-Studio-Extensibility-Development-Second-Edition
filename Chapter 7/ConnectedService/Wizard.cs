// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using ConnectedService.ViewModels;
using Microsoft.VisualStudio.ConnectedServices;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectedService
{
    internal class Wizard : ConnectedServiceWizard
    {
        private readonly ConnectedServiceProviderContext _context;

        public Wizard(ConnectedServiceProviderContext context)
        {
            _context = context;
            Pages.Add(new WizardPage(_context));

            foreach (var page in Pages)
            {
                page.PropertyChanged += OnPagePropertyChanged;
            }
        }

        private void OnPagePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IsFinishEnabled = Pages.All(page => !page.HasErrors);
            IsNextEnabled = false;
        }

        public override Task<ConnectedServiceInstance> GetFinishedServiceInstanceAsync()
        {
            var instance = new Instance
            {
                Country = Pages.OfType<WizardPage>().FirstOrDefault()?.Country
            };

            return Task.FromResult<ConnectedServiceInstance>(instance);
        }
    }
}
