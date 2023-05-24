using System.Security.Principal;

namespace Snippets.Helpers
{
    public class ElevationHelper
    {
        private readonly bool _isElevated;

        public bool IsElevated => _isElevated;

        public ElevationHelper()
        {
            _isElevated = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
