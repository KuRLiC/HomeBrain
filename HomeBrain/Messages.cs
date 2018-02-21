using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeBrain
{
    [Serializable]
    public abstract class IMessage
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return "{0} - {1} - {2}".F(Id, Timestamp, GetType().Name);
        }

        protected virtual void Init()
        {
            Id = Guid.NewGuid();
            OwnerId = Brain.ID;
            Timestamp = DateTime.Now;
        }

        public static IMessage Parse(byte[] data)
        {
            try
            {
                if (data != null)
                    return data.FromBin<IMessage>();
            }
            catch { }
            return null;
        }
    }

    [Serializable]
    public class AliveMessage : IMessage
    {
        public string Username { get; set; }
        public string Host { get; set; }
        public string Domain { get; set; }
        public System.Net.IPAddress[] IPAddresses { get; set; }

        public static AliveMessage Create()
        {
            var msg = new AliveMessage();
            msg.Init();
            return msg;
        }

        protected override void Init()
        {
            base.Init();
            Host = Environment.MachineName;
            Username = Environment.UserName;
            Domain = Environment.UserDomainName;
            IPAddresses = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Where(x => !x.IsReceiveOnly && x.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up).SelectMany(x => x.GetIPProperties().UnicastAddresses.Select(y => y.Address)).ToArray();
        }

    }
}
