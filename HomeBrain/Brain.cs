using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeBrain
{
    public class Brain
    {
        public static FolderItem AppData { get; protected set; }
        public static Guid ID { get; set; }

        public static void Init()
        {
            // data folder
            AppData = new FolderItem(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)).Folder["HomeBrain"];
            // app id
            var idFile = AppData.File["ID"];
            if (!idFile.Exists)
                idFile.Text = Guid.NewGuid().ToString();
            ID = Guid.Parse(idFile.Text);
            // read files
            foreach (var alive in ReadDB())
                AliveDB.Add(alive.OwnerId, alive);
            // network setup
            Channel.Init();
            Channel.BroadcastReceived += new Channel.BroadcastReceivedDelegate(Channel_BroadcastReceived);
            // handle messages
            Message += new MessageDelegate(Brain_Message);
            // subscribe
            Subscribe<AliveMessage>(OnAliveRecvd);
        }

        #region subscriptions
        public class Subscription
        {
            public Delegate OriginalHandler;
            public Type MessageType;
            public Action<Exception> OnError;
        }
        public static List<Subscription> Subscriptions = new List<Subscription>();
        public static Subscription Subscribe<T>(Action<T> action) where T : IMessage
        {
            var subscription = new Subscription();
            subscription.MessageType = typeof(T);
            subscription.OriginalHandler = action;
            lock (Subscriptions) Subscriptions.Add(subscription);
            return subscription;
        }
        public static Subscription[] GetSubscription(Type messageType)
        {
            lock (Subscriptions)
                return Subscriptions.Where(x => x.MessageType == messageType).ToArray();
        }
        public static void Dispatch(IMessage msg)
        {
            var subs = GetSubscription(msg.GetType());
            foreach (var sub in subs)
            {
                try
                {
                    sub.OriginalHandler.DynamicInvoke(msg);
                }
                catch (Exception ex)
                {
                    if (sub.OnError != null)
                        sub.OnError(ex);
                }
            }
        }
        #endregion

        #region msg handler
        static void Brain_Message(IMessage msg)
        {
            Dispatch(msg);
        }

        public delegate void MessageDelegate(IMessage msg);
        public static event MessageDelegate Message;

        static void Channel_BroadcastReceived(System.Net.IPEndPoint clientEndpoint, byte[] data)
        {
            if (Message != null)
            {
                var msg = IMessage.Parse(data);
                if (msg != null)
                    Message(msg);
            }
        }
        #endregion

        #region alive
        public static void ImAlive()
        {
            Channel.SendBroadcast(AliveMessage.Create());
        }
        private static Dictionary<Guid, AliveMessage> AliveDB = new Dictionary<Guid, AliveMessage>();
        static void OnAliveRecvd(AliveMessage msg)
        {
            lock (AliveDB)
                if (!AliveDB.ContainsKey(msg.OwnerId))
                    AliveDB.Add(msg.OwnerId, msg);
                else
                    AliveDB[msg.OwnerId] = msg;
            AppData.Folder["Alive"].File["{0}.bin".F(msg.OwnerId)].Bytes = msg.ToBin();
        }
        static IEnumerable<AliveMessage> ReadDB()
        {
            return AppData.Folder["Alive"].Files.Select(x => x.Bytes.FromBin<AliveMessage>());
        }
        #endregion
    }
}
