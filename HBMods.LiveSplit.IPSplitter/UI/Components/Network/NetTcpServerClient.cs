using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace LiveSplit.UI.Components.Network
{
    public class NetTcpServerClient
    {
        public Action<NetTcpServerClient, EnumConnecitonState> OnClientConnectionStateChanged { get; set; }
        public Action<NetTcpServerClient, byte[]> OnClientDataRx { get; set; }
        public Action<NetTcpServerClient, string, Exception> OnException { get; set; }


        private const int _bufferSize = 2048;
        private byte[] _buffer = new byte[_bufferSize];


        private TcpClient Client { get; set; } = null;
        private Socket Socket { get; set; } = null;


        private EnumConnecitonState _connectionState = EnumConnecitonState.Closed;
        public EnumConnecitonState ConnectionState { get => _connectionState; set { if (_connectionState == value) return; _connectionState = value; OnClientConnectionStateChanged?.Invoke(this, _connectionState); } }


        public string ConnectionStr { get; private set; }

        public string IMEI { get; private set; }
        public void SetIMEI(string newIMEDI) => IMEI = newIMEDI;


        public void MonitorConnection(TcpClient tcpClient)
        {
            Client = tcpClient;
            Socket = tcpClient.Client;
            ConnectionStr = Socket.RemoteEndPoint.ToString();

            try
            {
                Socket.BeginReceive(_buffer, 0, _bufferSize, 0, new AsyncCallback(ReceiveCallback), null);

                ConnectionState = EnumConnecitonState.Open;

                return;
            }
            catch (IOException ex)
            {
                OnException?.Invoke(this, $"[MonitorConnection] Exception: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, $"[MonitorConnection] Exception: {ex.Message}", ex);
            }

            CloseConnection();
        }


        public void CloseConnection()
        {
            try
            {
                if (Client != null)
                {
                    if (ConnectionState != EnumConnecitonState.Closed)
                    {
                        Client?.Close();
                        Client?.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                // Ignore Exceptions
                OnException?.Invoke(this, $"[CloseConnection] Exception: {ex.Message}", ex);
            }

            ConnectionState = EnumConnecitonState.Closed;
        }


        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Read data from the remote device.  
                int bytesRead = Socket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    byte[] subBytes = new byte[bytesRead];
                    Array.Copy(_buffer, 0, subBytes, 0, bytesRead);
                    OnClientDataRx?.Invoke(this, subBytes);
                    Socket.BeginReceive(_buffer, 0, _bufferSize, 0, new AsyncCallback(ReceiveCallback), null);
                    return;
                }
            }
            catch (IOException ex)
            {
                OnException?.Invoke(this, $"[ReceiveCallback] Exception: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, $"[ReceiveCallback] Exception: {ex.Message}", ex);
            }

            CloseConnection();
        }


        public bool SendData(string command)
        {
            NetworkStream networkStream = Client?.GetStream();

            if (networkStream == null)
                return false;

            byte[] bytes = Encoding.UTF8.GetBytes(command);

            networkStream.Write(bytes, 0, bytes.Length);

            return true;
        }
    }
}
