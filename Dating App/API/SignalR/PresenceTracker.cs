namespace DatingApplication.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlinePresence = new Dictionary<string, List<string>>();

        public Task UserConnected(string userName, string connectionId)
        {
            lock (OnlinePresence)
            {
                if (OnlinePresence.ContainsKey(userName))
                {
                    OnlinePresence[userName].Add(connectionId);
                }
                else
                {
                    OnlinePresence.Add(userName, new List<string>() { connectionId});
                }
            }
            return  Task.CompletedTask;
        }

        public Task UserDisConnected(string userName, string connectionId)
        {
            lock (OnlinePresence)
            {
                if (!OnlinePresence.ContainsKey(userName)) return Task.CompletedTask;

                OnlinePresence[userName].Remove(connectionId);

                if (OnlinePresence[userName].Count == 0)
                {
                    OnlinePresence.Remove(userName);
                }
            }
            return Task.CompletedTask;
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (OnlinePresence)
            {
                onlineUsers = OnlinePresence.OrderBy(k=> k.Key).Select(k=> k.Key).ToArray();
            }
            return Task.FromResult(onlineUsers);
        }

        public static Task<List<string>> GetConnectionForUsers(string userName)
        {
            List<string> connectionIds;

            lock (OnlinePresence)
            {
                connectionIds = OnlinePresence?.GetValueOrDefault(userName);
            }

            return Task.FromResult(connectionIds);    
        }
    }
}
