using System.Net.NetworkInformation;

namespace Snippets
{
    public static class Networking
    {
        private static bool GetBoolFromPingStatus(IPStatus status) => status switch
        {
            IPStatus.Success => true,
            _ => false
        };

        // A Tuple allows us to have more values in output
        public static (bool, string) SendPing(string ipAddress)
        {
            // The using statement ensures the correct use of IDisposable objects.
            using (var ping = new Ping())
            {
                try
                {
                    var checkPing = ping.Send(ipAddress);
                    return (GetBoolFromPingStatus(checkPing.Status), checkPing.Status.ToString());
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
                    return (GetBoolFromPingStatus(checkPing.Status), checkPing.Status.ToString());
                }

                catch (Exception ex) { return (false, ex.Message); }
            }
        }
    }
}
