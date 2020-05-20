using System.Threading.Tasks;

namespace SocketIOClient.Parsers
{
    class HeartbeatParser : IParser
    {
        public Task ParseAsync(ResponseTextParser rtp)
        {
            rtp.Socket._isHeartbeatFinished = true;
            rtp.Socket._timeoutNumber = 0;
            rtp.Socket._heartbeatDelay = 0;
            rtp.Socket._isConnectTimeout = false;
            return rtp.Socket.InvokePongAsync();
        }
    }
}
