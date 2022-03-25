using System;
using System.Net;

namespace LiveSplit.UI.Components.Network
{
    public interface INetServer
    {
        event Action<INetServer, EnumConnecitonState> OnConnectionStateChange;
        event Action<INetServer, byte[]> OnMessageRx;
        event Action<INetServer, string> OnServerError;

        string Description { get; }
        EnumUdpServerType ServerType { get; }

        bool Close();
        bool Open(string ipAddressStr, int port, string nicIpStr);
        bool Open(IPAddress ipAddress, int port, IPAddress nicIp);

        EnumConnecitonState ConnecitonState { get; set; }
        string LastError { get; }
    }
}
