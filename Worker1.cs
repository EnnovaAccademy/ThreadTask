using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadEsempio
{
    public class Worker1 : IWorker
    {
        private Task threadToWait;
        public void WaitFor(Task threadToWait)
        {
            this.threadToWait = threadToWait;
        }
        public Task Initialize(Action<int> a)
        {
            var t1 = new Task(() =>
            {
                Console.WriteLine("[{0}] L1 THREAD START", Thread.CurrentThread.ManagedThreadId);
                for (int i = 0; i < 40; i++)
                {
                    if (a != null)
                        a(i);
                    Console.WriteLine("[{0}] L1  LOOP {1}", Thread.CurrentThread.ManagedThreadId, i);
                    Thread.Sleep(200);
                }
                Console.WriteLine("[{0}] L1 THREAD END", Thread.CurrentThread.ManagedThreadId);
            });

            return t1;
        }
    }
}
