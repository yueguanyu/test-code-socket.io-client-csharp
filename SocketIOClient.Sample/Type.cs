using Newtonsoft.Json;

namespace SocketIOClient.Sample
{
    public class User
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
    }
    public class AuthResult
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("user")]
        public User User { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
        public int code { get; set; }
    }
    public class RpcResult
    {
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("result")]
        public Result Result { get; set; }
        [JsonProperty("method")]
        public string Method { get; set; }
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
    [JsonObject(MemberSerialization.OptIn)]
    public class Result
    {
        [JsonProperty("instanceId")]
        public string InstanceId { get; set; }
        [JsonProperty("serverId")]
        public int ServerId { get; set; }
    }

    public class param
    {
        public string instanceId { get; set; }
        public string gameId { get; set; }
        public string gameType { get; set; }
        public int eventType { get; set; }
        public string state { get; set; }
    }

    public class Payload
    {
        public int id { get; set; }
        public string method { get; set; }
        [JsonProperty("params")]
        public param Params { get; set; }
    };

    public class UserGame
    {
        public string userId { get; set; }
        public string gameId { get; set; }
        public string date { get; set; }
    }

    public class StreamData
    {
        public string message { get; set; }
        public string method { get; set; }
        public UserGame usergame { get; set; }
    }

    public class StreamResult
    {
        public string instanceId { get; set; }
        public string gameEvent { get; set; }
        public StreamData data { get; set; }
    }
}