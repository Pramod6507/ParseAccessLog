using System.Linq;

namespace ParseAccessLog
{
    public static class AccessParseHelper
    {
        public static string IpAddressLabel(string ipAddress)
                => string.Join(".", ipAddress.Split('.').Select(part => part.PadLeft(3, '0')));


    }
}
