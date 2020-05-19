using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;

namespace SocketIOClient.Sample {

    class Program {
        System.Timers.Timer Timers_Timer = new System.Timers.Timer ();
        static public string userId;
        static async Task Main (string[] args) {
            //var client = new SocketIO("http://localhost:3000");
            //client.ConnectTimeout = TimeSpan.FromSeconds(5);

            //client.On("test", args =>
            //{
            //    string text = JsonConvert.DeserializeObject<string>(args.Text);
            //    Console.WriteLine(text);
            //});

            //client.OnConnected += async () =>
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        await client.EmitAsync("test", i.ToString());
            //        await Task.Delay(1000);
            //    }

            //    await client.EmitAsync("close", "close");
            //};

            //client.OnClosed += Client_OnClosed;

            //await client.ConnectAsync();

            //-----------------
            var player1 = new Player ("18534572861", "123456789");
            player1.initListener ();
            // player1.init();
            System.Threading.Thread.Sleep (5000);
            // var player2 = new Player ("18534572862", "qiushanyu666");
            // Console.WriteLine ($"player1.instanceId:{player1.instanceId}");
            // player2.init(player1.instanceId);

            Console.ReadLine ();
        }
    }
}