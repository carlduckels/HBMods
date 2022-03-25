using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LiveSplit.UI.Components.Network
{
    public class NetUdpServer : IDisposable, INetServer
    {
        public event Action<INetServer, EnumConnecitonState> OnConnectionStateChange;
        public event Action<INetServer, byte[]> OnMessageRx;
        public event Action<INetServer, string> OnServerError;

        EnumUdpServerType _serverType = EnumUdpServerType.Unknown;
        UdpClient _udpClient = null;
        int _port = 0;
        IPAddress _ipAddress = null;
        IPAddress _nicIpAddress = null;
        IPEndPoint _localEndPoint = null;


        string _lastError;
        EnumConnecitonState _connecitonState = EnumConnecitonState.Closed;


        public NetUdpServer()
        { }


        public void Dispose()
        {
            try
            {
                _udpClient?.Dispose();
            }
            catch (Exception)
            {
                // Ignore
            }
        }


        public string Description => $"UDP Server [{_ipAddress}:{_port} - {_connecitonState}]";
        public string LastError => _lastError;


        private void RaiseError(string errMsg)
        {
            _lastError = errMsg;
            OnServerError?.Invoke(this, _lastError);
            ConnecitonState = EnumConnecitonState.Error;
        }


        public EnumUdpServerType ServerType => _serverType;


        public EnumConnecitonState ConnecitonState
        {
            get => _connecitonState;
            set
            {
                if (_connecitonState == value)
                    return;
                _connecitonState = value;

                OnConnectionStateChange?.Invoke(this, _connecitonState);
            }
        }


        public bool Open(string ipAddressstr, int port, string nicIpStr = null)
        {
            try
            {
                if (nicIpStr == null)
                    return Open(IPAddress.Parse(ipAddressstr), port);
                else
                    return Open(IPAddress.Parse(ipAddressstr), port, IPAddress.Parse(nicIpStr));
            }
            catch (Exception)
            {
                RaiseError($"IP Address is Invalid ('{ipAddressstr}' / '{nicIpStr}')");
                return false;
            }
        }


        public bool Open(IPAddress ipAddress, int port, IPAddress nicIp = null)
        {
            _lastError = "";

            _serverType = NetHelper.UDPServerType(ipAddress);
            _ipAddress = ipAddress;
            _port = port;

            switch (_serverType)
            {
                case EnumUdpServerType.Invalid:
                case EnumUdpServerType.Unknown:
                    RaiseError($"Server Type {_serverType}   [from {ipAddress}]");
                    return false;
                case EnumUdpServerType.UdpUnicastServer:
                    nicIp = ipAddress;
                    break;
                case EnumUdpServerType.UdpMulticastServer:
                case EnumUdpServerType.UdpBroadcastServer:
                    if (nicIp == null)
                        nicIp = IPAddress.Any;
                    break;
                default:
                    break;

            }

            _nicIpAddress = nicIp;

            try
            {
                ConnecitonState = EnumConnecitonState.Opening;

                // Create endpoints
                _localEndPoint = new IPEndPoint(_nicIpAddress, _port);

                // Create and configure UdpClient
                _udpClient = new UdpClient();

                // The following three lines allow multiple clients on the same PC
                _udpClient.ExclusiveAddressUse = false;
                _udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _udpClient.ExclusiveAddressUse = false;

                // Bind, Join
                _udpClient.Client.Bind(_localEndPoint);

                switch (_serverType)
                {
                    case EnumUdpServerType.UdpUnicastServer:
                        Console.WriteLine($"Opening UDP Unicast Server: {_ipAddress}:{_port}   [Local End Point: {_nicIpAddress.ToFriendlyName()}]");
                        break;
                    case EnumUdpServerType.UdpMulticastServer:
                        Console.WriteLine($"Opening UDP Multicast Server: {_ipAddress}:{_port}   [Local End Point: {_nicIpAddress.ToFriendlyName()}]");
                        _udpClient.JoinMulticastGroup(_ipAddress, _nicIpAddress);
                        break;
                    case EnumUdpServerType.UdpBroadcastServer:
                        Console.WriteLine($"Opening UDP Broadcast Server: {_ipAddress}:{_port}   [Local End Point: {_nicIpAddress.ToFriendlyName()}]");
                        _udpClient.EnableBroadcast = true;
                        break;
                    default:
                        break;
                }

                // Start listening for incoming data
                _udpClient.BeginReceive(new AsyncCallback(ReceivedCallback), null);

                ConnecitonState = EnumConnecitonState.Open;

                return true;
            }
            catch (Exception ex)
            {
                RaiseError($"Error Opening: {ex.Message}");
                Close();
            }

            return false;
        }


        public bool Close()
        {
            ConnecitonState = EnumConnecitonState.Closing;

            if (_udpClient != null)
            {
                var client = _udpClient;
                _udpClient = null;
                client.Close();
                client.Dispose();
            }

            ConnecitonState = EnumConnecitonState.Closed;

            return true;
        }


        private void ReceivedCallback(IAsyncResult ar)
        {
            try
            {
                IPEndPoint sender = new IPEndPoint(0, 0);

                if (_udpClient == null)
                    return;

                byte[] receivedBytes = _udpClient.EndReceive(ar, ref sender);

                OnMessageRx?.Invoke(this, receivedBytes);

                if (_udpClient == null)
                {
                    Close();
                }
                else
                {
                    _udpClient.BeginReceive(new AsyncCallback(ReceivedCallback), null);
                }

                return;
            }
            catch (Exception ex)
            {
                RaiseError($"Error Receiving: {ex.Message}");
                Close();
            }
        }
    }
}
