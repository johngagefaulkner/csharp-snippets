namespace Snippets.Helpers
{
    public partial class ElevationHelper
    {
        public static bool IsCurrentProcessElevated() => new System.Security.Principal.WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }
}
