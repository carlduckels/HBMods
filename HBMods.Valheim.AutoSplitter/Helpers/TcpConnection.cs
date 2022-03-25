using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace HBMods.Valheim.AutoSplitter.Helpers
{
    internal static class TcpConnection
    {
        public static bool IsConnected { get; private set; }
        
        private static TcpClient _tcpClient = null;
        private static NetworkStream _networkStream = null;


        public static void Connect(string ipAddress, int port)
        {
            Task.Run(() =>
            {
                try
                {
                    if (_tcpClient != null)
                    {
                        Close();
                    }

                    _tcpClient = new TcpClient(ipAddress, port);

                    _networkStream = _tcpClient.GetStream();

                    Debug.Log("TCP Connection Successful");

                    IsConnected = true;

                    return;
                }
                catch (Exception)
                {
                    Debug.LogWarning($"TCP Connection Failed - Can't Connect to LiveSplit TCP Server on {ipAddress}:{port}");
                }

                IsConnected = false;
            });
        }


        public static void Close()
        {
            _networkStream.Close();
            _tcpClient.Close();

            _networkStream.Dispose();
            _tcpClient.Dispose();

            _networkStream = null;
            _tcpClient = null;
        }


        public static void Send(string str)
        {
            if (_networkStream == null)
            {
                Debug.Log($"Not Sending Text:  '{str}'   [Network Stream is not Open]");
                return;
            }

            Debug.Log($"Sending Text:  '{str}'");

            byte[] data = Encoding.UTF8.GetBytes(str);
            
            _networkStream.Write(data, 0, data.Length);
        }
    }
}
