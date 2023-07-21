using System.Net.NetworkInformation;

namespace Snippets
{
    public static class Networking
    {
        // A Tuple allows us to have more values in output
        public static (bool, string) SendPing(string ipAddress)
        {
            // The using statement ensures the correct use of IDisposable objects.
            using (var ping = new Ping())
            {
                try
                {
                    var checkPing = ping.Send(ipAddress);
                    return (IPStatus.Success, checkPing.Status.ToString());
                }
                catch (Exception ex) { return (false, ex.Message); }
            }
        }

        // Tuple allows more values in output, Async allows not locking UI thread
        public static async Task<(bool, string)> SendPingAsync(string ipAddress)
        {
            using (var ping = new Ping()) // The "using" statement ensures correct use of IDisposable objects
            {
                try
                {
			var checkPing = await ping.SendPingAsync(ipAddress);
 			return (IPStatus.Success, checkPing.Status.ToString());
                }

                catch (Exception ex) { return (false, ex.Message); }
            }
        }
    }
}
