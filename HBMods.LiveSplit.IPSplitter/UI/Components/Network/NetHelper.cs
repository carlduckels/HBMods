using System;
using System.Net;

namespace LiveSplit.UI.Components.Network
{
    public static class NetHelper
    {
        public static EnumUdpServerType UDPServerType(string ipAddressStr)
        {
            try
            {
                return UDPServerType(IPAddress.Parse(ipAddressStr));
            }
            catch (Exception)
            {
                return EnumUdpServerType.Invalid;
            }
        }


        public static EnumUdpServerType UDPServerType(IPAddress ipAddress)
        {
            byte[] ipAddressBytes = ipAddress.GetAddressBytes();

            if ((ipAddressBytes[0] == 255) && (ipAddressBytes[1] == 255) && (ipAddressBytes[2] == 255) && (ipAddressBytes[3] == 255))
                return EnumUdpServerType.UdpBroadcastServer;

            if ((ipAddressBytes[0] >= 224) && (ipAddressBytes[0] <= 239))
                return EnumUdpServerType.UdpMulticastServer;

            return EnumUdpServerType.UdpUnicastServer;
        }


        public static string ToFriendlyName(this IPAddress ipAddress)
        {
            byte[] ipAddressBytes = ipAddress.GetAddressBytes();

            if ((ipAddressBytes[0] == 255) && (ipAddressBytes[1] == 255) && (ipAddressBytes[2] == 255) && (ipAddressBytes[3] == 255))
                return "Broadcast";

            if ((ipAddressBytes[0] == 0) && (ipAddressBytes[1] == 0) && (ipAddressBytes[2] == 0) && (ipAddressBytes[3] == 0))
                return "Any";

            if ((ipAddressBytes[0] >= 224) && (ipAddressBytes[0] <= 239))
                return $"Multicast[{ipAddress}]";

            return ipAddress.ToString();
        }
    }
}
