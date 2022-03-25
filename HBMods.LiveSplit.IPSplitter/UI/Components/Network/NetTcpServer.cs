using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace LiveSplit.UI.Components.Network
{
    public class NetTcpServer
    {
        public Action<NetTcpServer, EnumConnecitonState> OnConnectionStateChanged;
        public Action<NetTcpServer, string, Exception> OnException;
        public Action<NetTcpServer, NetTcpServerClient> OnClientAdded;
        public Action<NetTcpServer, NetTcpServerClient> OnClientRemoved;

        public Action<NetTcpServer, NetTcpServerClient, EnumConnecitonState> OnClientConnectionStateChanged;
        public Action<NetTcpServer, NetTcpServerClient, byte[]> OnClientDataRx;
        public Action<NetTcpServer, NetTcpServerClient, string, Exception> OnClientException;

        private TcpListener _tcpListener = null;
        private List<NetTcpServerClient> _tcpClients = new List<NetTcpServerClient>();

        public List<NetTcpServerClient> TcpClients => _tcpClients?.ToList();

        public int TcpPort { get; private set; } = 0;
        public IPAddress IpAddress { get; private set; } = IPAddress.Any;
        public string ConnectionStr { get; private set; } = "Closed";

        private EnumConnecitonState _connectionState = EnumConnecitonState.Closed;
        public EnumConnecitonState ConnectionState { get => _connectionState; set { if (_connectionState == value) return; _connectionState = value; OnConnectionStateChanged?.Invoke(this, _connectionState); } }


        public void OpenTcpListener(int tcpPort) => OpenTcpListener(IPAddress.Any, tcpPort);


        public void OpenTcpListener(IPAddress ipAddress, int tcpPort)
        {
            try
            {
                ConnectionState = EnumConnecitonState.Opening;

                IpAddress = ipAddress;
                TcpPort = tcpPort;

                if (_tcpListener == null)
                    _tcpListener = new TcpListener(IpAddress, TcpPort);

                _tcpListener.Start();

                _tcpListener.BeginAcceptTcpClient(ProcessTcpClient, _tcpListener);

                ConnectionStr = $"{IpAddress}:{TcpPort}";

                ConnectionState = EnumConnecitonState.Open;

                return;
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, $"[OpenTcpListener] Exception: {ex.Message}", ex);
            }

            CloseTcpListener();
        }


        public void CloseTcpListener()
        {
            try
            {
                if (_tcpListener != null)   // Close Listener if it exists
                {
                    ConnectionState = EnumConnecitonState.Closing;

                    _tcpListener.Stop();
                    _tcpListener = null;

                    foreach (NetTcpServerClient client in _tcpClients)
                    {
                        client.CloseConnection();
                    }

                    _tcpClients.Clear();
                }
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, $"[CloseTcpListener] Exception: {ex.Message}", ex);
            }

            ConnectionState = EnumConnecitonState.Closed;
        }


        private void ProcessTcpClient(IAsyncResult ar)
        {
            try
            {
                if (_tcpListener == null)
                    return;

                var tcpClient = _tcpListener.EndAcceptTcpClient(ar);

                _tcpListener.BeginAcceptTcpClient(ProcessTcpClient, _tcpListener);

                AddTcpClientConnection(tcpClient);
            }
            catch (ObjectDisposedException ex)
            {
                OnException?.Invoke(this, $"[ProcessTcpClient] Exception: {ex.Message}", ex);
            }
            catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
            {
                OnException?.Invoke(this, $"[ProcessTcpClient] Exception: {ex.Message}", ex);
            }
            catch (SocketException ex)
            {
                OnException?.Invoke(this, $"[ProcessTcpClient] Exception: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, $"[ProcessTcpClient] Exception: {ex.Message}", ex);
            }
        }


        private void AddTcpClientConnection(TcpClient tcpClient)
        {
            if (tcpClient == null)
                return;

            NetTcpServerClient newClient = new NetTcpServerClient();

            newClient.OnClientConnectionStateChanged = TcpClient_OnConnectionStateChanged;
            newClient.OnClientDataRx += TcpClient_OnDataRx;
            newClient.OnException += TcpClient_OnException;

            _tcpClients.Add(newClient);

            newClient.MonitorConnection(tcpClient);

            OnClientAdded?.Invoke(this, newClient);
        }


        private void RemoveTcpClientConnection(NetTcpServerClient client)
        {
            if (client == null)
                return;

            client.OnClientConnectionStateChanged = null;
            client.OnClientDataRx = null;
            client.CloseConnection();

            if (_tcpClients.Contains(client))
            {
                _tcpClients.Remove(client);
                OnClientRemoved?.Invoke(this, client);
            }
        }


        private void TcpClient_OnConnectionStateChanged(NetTcpServerClient client, EnumConnecitonState newState)
        {
            OnClientConnectionStateChanged?.Invoke(this, client, newState);

            if ((newState == EnumConnecitonState.Closed) || (newState == EnumConnecitonState.Error))
            {
                RemoveTcpClientConnection(client);
            }
        }


        private void TcpClient_OnDataRx(NetTcpServerClient client, byte[] data) => OnClientDataRx?.Invoke(this, client, data);


        private void TcpClient_OnException(NetTcpServerClient client, string info, Exception ex) => OnClientException?.Invoke(this, client, info, ex);


        public NetTcpServerClient FindClientByIMEI(string imei)
        {
            return _tcpClients.FirstOrDefault(x => x.IMEI == imei);
        }


        public NetTcpServerClient FindClientByConnectionStr(string connectionStr)
        {
            return _tcpClients.FirstOrDefault(x => x.ConnectionStr == connectionStr);
        }
    }
}
