using System;
using System.Net.NetworkInformation;
 
//namespace Snippets.Networking
namespace Snippets
{
    //internal class CheckService
    public partial class Networking
    {
        // A Tuple allows us to have more values in output
        public (bool, string) IsReachable(string ipOrAddress)
        {
            // The using statement ensures the correct use of IDisposable objects.
            using (var ping = new Ping())
            {
                try
                {
                    bool result = true;
                    // In the method Send we can pass the IP or the Address
                    var checkPing = ping.Send(ipOrAddress);
                    if (checkPing.Status != IPStatus.Success)
                    {
                        result = false;
                    }
 
                    return (result, checkPing.Status.ToString());
                }
                catch(Exception ex)
                {
                    return (false, ex.Message) ;
                }
            }
        }
    }
}
