using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadEsempio
{
    public interface IWorker
    {
        Task Initialize(Action<int> a);
    }
    public class Worker2 : IWorker
    {
        public Task Initialize(Action<int> a)
        {
            var t2 = new Task(() =>
            {
                Console.WriteLine("[{0}] L2 THREAD START", Thread.CurrentThread.ManagedThreadId);
                for (int i = 0; i < 60; i++)
                {
                    if (a != null)
                        a(i);
                    Console.WriteLine("[{0}] L2  LOOP {1}", Thread.CurrentThread.ManagedThreadId, i);
                    Thread.Sleep(200);
                }
                Console.WriteLine("[{0}] L2 THREAD END", Thread.CurrentThread.ManagedThreadId);
            });
            
            return t2;
        }
    }
}
