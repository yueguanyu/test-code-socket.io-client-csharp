using System.Collections.Generic;

namespace SocketIOClient.Arguments
{
    public class Option
    {
        public int HeartbeatTimeoutDelay { get; set; } = 2000;
        public int TimeoutTimesForClose { get; set; } = 2;
    }
}
