using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace HomeBrain
{
    public class Channel
    {
        public const int BROADCAST_PORT = 15726;

        public static void Signal()
        {

        }

        public static void SendBroadcast(object obj)
        {
            var bytes = obj.ToBin();
            var compressed = bytes.Compress();
            if (compressed.Length >= bytes.Length)
            {
                System.Diagnostics.Debugger.Break();
            }
            SendBroadcast(compressed);
        }
        public static void SendBroadcast(byte[] bytes)
        {
            using (var client = new UdpClient())
            {
                client.EnableBroadcast = true;
                var ip = new IPEndPoint(IPAddress.Broadcast, BROADCAST_PORT);
                client.Send(bytes, bytes.Length, ip);
                client.Close();
            }
        }

        public static void BeginReceviceBroadcast()
        {
            var server = new UdpClient(BROADCAST_PORT);
            server.EnableBroadcast = true;
            while (true)
            {
                var clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
                var data = server.Receive(ref clientEndpoint);
                ThreadEx.ExecTask(OnBroadcastReceived, clientEndpoint, data);
            }
        }

        public delegate void BroadcastReceivedDelegate(IPEndPoint clientEndpoint, byte[] data);
        public static event BroadcastReceivedDelegate BroadcastReceived;
        private static void OnBroadcastReceived(IPEndPoint clientEndpoint, byte[] data)
        {
            var decompressed = data.Decompress();
            if (BroadcastReceived != null)
                BroadcastReceived(clientEndpoint, decompressed);
        }

        public static void Init()
        {
            ThreadEx.ExecThread(Channel.BeginReceviceBroadcast);
        }
    }
}
