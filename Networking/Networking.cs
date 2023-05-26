using System.Net.NetworkInformation;

namespace Snippets
{
    public partial class Networking
    {
		// Tuple allows more values in output, Async allows not locking UI thread
        public static async Task<(bool, string)> IsIPAddressReachable(string ipAddress)
        {
            using (var ping = new Ping()) // The "using" statement ensures correct use of IDisposable objects
            {
                try
                {
                    var checkPing = await ping.SendPingAsync(ipAddress);

                    if (checkPing.Status == IPStatus.Success) return (true, checkPing.Status.ToString());
                    else return (false, checkPing.Status.ToString());
                }

                catch (Exception ex)
                {
                    return (false, ex.Message);
                }
            }
        }
    }
}
