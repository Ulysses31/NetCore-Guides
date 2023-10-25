using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalOpenApiExample
{
  /// <summary>
  /// Shared class
  /// </summary>
  public class Shared
  {
    /// <summary>
    /// GetHostIpAddress function
    /// </summary>
    /// <returns>String</returns>
    public string GetHostIpAddress()
    {
      IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

      foreach (var item in ipHostInfo.AddressList)
      {
        if (item.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
          IPAddress ipAddress = item;
          return ipAddress.ToString();
        }
      }

      return String.Empty;
    }
  }
}