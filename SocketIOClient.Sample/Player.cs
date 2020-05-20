using System;
using Newtonsoft.Json;

namespace SocketIOClient.Sample
{
    class Player
    {
        public string url = Constants.SERVER_ADDR;
        private string email;
        private string password;
        public SocketIO client;
        public string userId;
        public string instanceId = "";
        public int serverId;
        public Player(string email, string password)
        {
            this.email = email;
            this.password = password;
            this.client = new SocketIO(url) { };
            Console.WriteLine($"player client init");
        }

        public async void initListener()
        {
            client.OnConnected += () =>
            {
                Console.WriteLine("connected");
            };
            client.OnClosed += reason =>
            {
                Console.WriteLine($"closed:{reason}");
            };
            client.OnError += error =>
            {
                Console.WriteLine($"error{error}");
            };
            client.OnPing += () =>
            {
                Console.WriteLine("ping");
            };
            client.OnPong += (pong) =>
            {
                Console.WriteLine($"pong:{pong}");
            };
            client.OnConnectTimeout += (timeout) =>
            {
                Console.WriteLine($"timeout:{timeout}");
            };
            await this.client.ConnectAsync();
        }

        public async void init(string instanceId = "")
        {
            if (instanceId != "")
            {
                this.instanceId = instanceId;
            }
            client.OnConnected += async () =>
            {
                var obj = new
                {
                    strategy = "local",
                    email = this.email,
                    password = this.password,
                    avoidUpdateToken = true,
                };

                await this.client.EmitAsync("authentication", obj, (res) =>
                {
                    Console.WriteLine($"res.Text:{res.Text}");
                    var authObj = JsonConvert.DeserializeObject<AuthResult>(res.Text);
                    Console.WriteLine($"authObj{authObj}");
                    this.userId = authObj.User.Id;
                    if (this.instanceId == "")
                    {
                        this.createRoom();
                    }
                    else
                    {
                        this.joinRoom(this.instanceId);
                    }
                    // await client.CloseAsync();
                });
            };
            await this.client.ConnectAsync();
        }

        public void listenAllEvents()
        {
            client.On("App\\Events\\GlassRowChanged", res =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(res.Text);
            });
            client.On("duplicate login", sid =>
            {
                Console.WriteLine($"duplicate login{sid}");
            });
            client.On("data", data =>
            {
                Console.WriteLine($"data{data.Text}");
            });
            client.On("rpc-call", rpcData =>
            {
                Console.WriteLine($"rpcData: {rpcData}");
                var rpcObj = JsonConvert.DeserializeObject<RpcResult>(rpcData.Text);
                if (rpcObj.Method == "real_time_game.SetGameBegin")
                {
                    // 游戏可以开始了
                    beginGame(instanceId);
                }
            });

            client.On("stream-call real_time_game.GameEventSyncSceneInstance", streamCallData =>
            {
                Console.WriteLine($"listenAllEvents -> stream-call client {this.userId} streamCallData{streamCallData.Text}");
                var streamObj = JsonConvert.DeserializeObject<StreamResult>(streamCallData.Text);
                if (streamObj.gameEvent == "START")
                {
                    sendSubmit();
                }
            });
        }

        public async void createRoom()
        {
            var obj = new Payload() { method = "real_time_game.JoinSceneInstance", id = 4, Params = new param() { instanceId = "", gameId = Constants.GAME_ID, gameType = Constants.GAME_TYPE, eventType = 0, } };
            await this.client.EmitAsync("rpc-call", obj, (res) =>
            {

                Console.WriteLine($"createRoom callBack:{res.Text}");
                var rpcObj = JsonConvert.DeserializeObject<RpcResult>(res.Text);
                this.instanceId = rpcObj.Result.InstanceId;
                this.serverId = rpcObj.Result.ServerId;
                Console.WriteLine($"createRoom success with instanceId: {this.instanceId}");
                this.listenAllEvents();
                this.setState();
            });
        }

        public async void joinRoom(string instanceId)
        {
            Console.WriteLine($"want join room:{instanceId}");
            var obj = new Payload() { method = "real_time_game.JoinSceneInstance", id = 4, Params = new param() { instanceId = this.instanceId, gameId = Constants.GAME_ID, gameType = Constants.GAME_TYPE, eventType = 1, } };

            await this.client.EmitAsync("rpc-call", obj, async (joinRes) =>
            {

                Console.WriteLine($"joinRoom callBack:{joinRes.Text}");
                var rpcObj = JsonConvert.DeserializeObject<RpcResult>(joinRes.Text);
                if (rpcObj.Error != null)
                {
                    Console.WriteLine($"joinRoom error with instanceId: {instanceId}");
                }
                else
                {
                    this.instanceId = rpcObj.Result.InstanceId;
                    this.listenAllEvents();
                    this.setState();
                }
            });
        }

        public async void setState()
        {
            var obj = new Payload() { method = "real_time_game.SetPlayerState", id = 4, Params = new param() { instanceId = this.instanceId, gameId = Constants.GAME_ID, gameType = Constants.GAME_TYPE, state = "ready", } };
            await this.client.EmitAsync("rpc-call", obj, async (setRes) =>
            {
                Console.WriteLine($"SetPlayerState -> client1 setStateRes:{setRes}");
            });
        }

        public async void beginGame(string instanceId)
        {
            var obj = new
            {
                method = "real_time_game.GameEventSyncSceneInstance",
                data = new
                {
                    serverId = this.serverId,
                },
            };
            await this.client.EmitAsync("stream-call", obj, syncRes =>
            {
                Console.WriteLine($"syncRes: {syncRes}");
                this.sendStart();
            });
        }

        public async void sendStart()
        {
            var obj = new
            {
                method = "real_time_game.GameEventSyncSceneInstance",
                data = new
                {
                    method = "start",
                    data = new
                    {
                        instanceId = this.instanceId,
                        playerId = this.userId,
                        gameId = Constants.GAME_ID,
                    },
                },
            };
            await this.client.EmitAsync("stream-call send", obj, sendRes =>
            {

                Console.WriteLine($"sendRes: {sendRes}");
            });
        }

        public async void sendSubmit()
        {
            var obj = new
            {
                method = "real_time_game.GameEventSyncSceneInstance",
                data = new
                {
                    method = "submit",
                    data = new
                    {
                        instanceId = this.instanceId,
                        playerId = this.userId,
                        gameId = Constants.GAME_ID,
                        moduleId = "A_5",
                        value = 0,
                    },
                },
            };
            await this.client.EmitAsync("stream-call send", obj, sendRes =>
            {
                Console.WriteLine($"sendSubmitRes: {sendRes}");
            });
        }
    }
}