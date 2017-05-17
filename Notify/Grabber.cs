using Screeps.ApiClient;

namespace Screeps.Notify
{
    public delegate void NotificationHandler(int tick, string message);
    public class Grabber
    {
        Client client = null;
        int DeleteLimit = 0;

        public event NotificationHandler OnNotification = (a,b) => { };

        public int Interval { get; set; }

        public Grabber(string screepsUsername, string screepsPassword)
        {
            client = new Client(screepsUsername, screepsPassword);
        }

        public int Poll()
        {
            int notifCount = ProcessNotifications();
            if (notifCount > 0)
            {
                ClearNotifications();
            }
            return notifCount;
        }

        int ProcessNotifications()
        {
            int count = 0;
            dynamic memory = client.UserMemoryGet("__notify");
            foreach (dynamic notification in memory.data)
            {
                int tick = notification.tick;
                OnNotification(tick, notification.message);
                if (DeleteLimit < tick) DeleteLimit = tick;
                count++;
            }
            return count;
        }
        
        void ClearNotifications()
        {
            string command = string.Format("(()=>{{Memory.__notify=Memory.__notify.filter((notification)=>notification.tick>{0});return \"Notifications collected\";}})();", DeleteLimit);
            client.UserConsole(command);
        }
    }
}
