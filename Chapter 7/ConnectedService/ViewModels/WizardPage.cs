// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Microsoft.VisualStudio.ConnectedServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ConnectedService.ViewModels
{
    public class WizardPage : ConnectedServiceWizardPage
    {
        private readonly ConnectedServiceProviderContext _context;

        private readonly IDictionary<string, object> _metadata;

        private readonly string[] propertyNames = { nameof(Country) };

        public string Country
        {
            get => GetProperty<string>();
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SetProperty("India");
                }
                else
                {
                    SetProperty(value);
                }
            }
        }

        public WizardPage(ConnectedServiceProviderContext context)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            _metadata = new Dictionary<string, object>
            {
                {nameof(Country), null}
            };

            _context = context;
            
            Title = Constants.Name;
            Description = Constants.Description;
            Legend = "Country";
            View = new Views.WizardPage
            {
                DataContext = this
            };
        }

        protected override void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (propertyNames.Contains(propertyName))
            {
                HasErrors = !IsPageValid();
            }

            base.OnPropertyChanged(propertyName);
        }

        private bool IsPageValid() => !string.IsNullOrWhiteSpace(Country);

        private void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
            {
                return;
            }

            if (!_metadata.ContainsKey(propertyName))
            {
                _metadata.Add(propertyName, value);
                OnPropertyChanged(propertyName);
            }
            else
            {
                var currentValue = (T)_metadata[propertyName];
                if (!EqualityComparer<T>.Default.Equals(currentValue, value))
                {
                    _metadata[propertyName] = value;
                    OnPropertyChanged(propertyName);
                }
            }
        }

        private T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null && _metadata.ContainsKey(propertyName))
            {
                var currentValue = (T)_metadata[propertyName];
                return currentValue;
            }

            return default;
        }
    }
}
