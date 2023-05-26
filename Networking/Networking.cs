using System.Net.NetworkInformation;

namespace Snippets
{
    public partial class Networking
    {
		public static async Task<(bool, string)> IsIPAddressReachable(string ipAddress) // Tuple allows more values in output, Async allows not locking UI thread
		{
			try
			{
				using (var ping = new Ping()) // "using" statement ensures correct use of IDisposable objects
				{
					var checkPing = await ping.SendPingAsync(ipAddress);
					return checkPing.Status == IPStatus.Success ? (true, checkPing.Status.ToString()) : (false, checkPing.Status.ToString());
				}
			}
			catch(Exception ex)
			{
				return (false, ex.Message) ;
			}
		}
    }
}
