using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal class Program
    {
        static void Test(string[] args)
        {
            Console.WriteLine("我是主线程，线程ID：" + Thread.CurrentThread.ManagedThreadId);
            //task用法一  
            Task task1 = new Task(() => MyAction());
            task1.Start();

            //task用法二  
            var strRes = Task.Run<string>(() => { return GetReturnStr(); });
            Console.WriteLine(strRes.Result);

            //task->async异步方法和await，主线程碰到await时会立即返回，继续以非阻塞形式执行主线程下面的逻辑  
            Console.WriteLine("---------------------------------");
            Console.WriteLine("①我是主线程，线程ID：{0}", Thread.CurrentThread.ManagedThreadId);
            var testResult = TestAsync();
            Console.ReadKey();
        }

        static async Task TestAsync()
        {
            Console.WriteLine("②调用GetReturnResult()之前，线程ID：{0}。当前时间：{1}", Thread.CurrentThread.ManagedThreadId,
                DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"));
            var name = GetReturnResult();
            Console.WriteLine("④调用GetReturnResult()之后，线程ID：{0}。当前时间：{1}", Thread.CurrentThread.ManagedThreadId,
                DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"));
            Console.WriteLine("⑥得到GetReturnResult()方法的结果一：{0}。当前时间：{1}", await name,
                DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"));
            Console.WriteLine("⑥得到GetReturnResult()方法的结果二：{0}。当前时间：{1}", name.GetAwaiter().GetResult(),
                DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss"));
        }

        static async Task<string> GetReturnResult()
        {
            Console.WriteLine("GetReturnResult sleep 3s");
            Thread.Sleep(3000);
            Console.WriteLine("③执行Task.Run之前, 线程ID：{0}", Thread.CurrentThread.ManagedThreadId);
            return await Task.Run(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("⑤GetReturnResult()方法里面线程ID: {0}", Thread.CurrentThread.ManagedThreadId);
                return "我是返回值";
            });
        }

        static void MyAction()
        {
            Console.WriteLine("我是新进程，线程ID：() MyAction " + Thread.CurrentThread.ManagedThreadId);
        }

        static string GetReturnStr()
        {
            Thread.Sleep(5000);
            return "我是返回值 ，GetReturnStr 线程ID：" + Thread.CurrentThread.ManagedThreadId;
        }
    }
}