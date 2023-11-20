// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Microsoft.VisualStudio.ConnectedServices;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ConnectedService
{
    public class Instance : ConnectedServiceInstance
    {
        public Instance()
        {
            this.InstanceId = Constants.Category;
            this.Name = Constants.Name;
        }

        public string Country
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        private void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName == null)
            {
                return;
            }

            if (!Metadata.ContainsKey(propertyName))
            {
                Metadata.Add(propertyName, value);
                this.OnPropertyChanged(propertyName);
            }
            else
            {
                var currentValue = (T)Metadata[propertyName];
                if (!EqualityComparer<T>.Default.Equals(currentValue, value))
                {
                    Metadata[propertyName] = value;
                    OnPropertyChanged(propertyName);
                }
            }
        }

        private T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName != null && Metadata.ContainsKey(propertyName))
            {
                var currentValue = (T)Metadata[propertyName];
                return currentValue;
            }

            return default;
        }
    }
}
