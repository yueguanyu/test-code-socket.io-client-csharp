using System.Threading.Tasks;

namespace SocketIOClient.Parsers
{
    class HeartbeatParser : IParser
    {
        public Task ParseAsync(ResponseTextParser rtp)
        {
            rtp.Socket._isHeartbeatFinished = true;
            return rtp.Socket.InvokePongAsync();
        }
    }
}
