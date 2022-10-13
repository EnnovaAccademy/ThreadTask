namespace ThreadEsempio
{
    internal class Program
    {
        static SemaphoreSlim semaphore;
        static async Task Main(string[] args)
        {
            semaphore = new SemaphoreSlim(1);
            Console.WriteLine("[{0}] PROGRAM START", Thread.CurrentThread.ManagedThreadId);
            List<int> numbers = new List<int>();
            object _lock = new();
            for (var i = 0; i < 10000; i++)
            {
                numbers.Add(i);
            }
            var worker1 = new Worker1();
            var worker2 = new Worker2();
            Task t1 = null;
            Task t2 = null;
            t1 = worker1.Initialize(async i =>
            {
                semaphore.Wait();
                try
                {
                    numbers.Remove(numbers.Count - 1);
                }
                finally
                {
                    semaphore.Release();
                }

                if (t2 != null && i == 19)
                {
                    await t2;
                    Console.WriteLine("[{0}] WAITING FOR T2 TO END", Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("[{0}] T2 ENDED, CONTINUING...", Thread.CurrentThread.ManagedThreadId);
                }
            });
            t1.Start();
            t2 = worker2.Initialize(async i =>
            {
                var sum = 0;

                semaphore.Wait();
                try
                {
                    foreach (var j in numbers)
                    {
                        sum += j;
                    }
                }
                finally
                {
                    semaphore.Release();
                }



                Console.WriteLine("[{0}] SUM={1}", Thread.CurrentThread.ManagedThreadId, sum);
            });
            worker1.WaitFor(t2);
            t2.Start();
            Console.WriteLine("Started worker T2");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("[{0}] PROGRAM LOOP {1}", Thread.CurrentThread.ManagedThreadId, i);
                Thread.Sleep(200);
            }
            await t1;
            Console.WriteLine("[{0}] PROGRAM END", Thread.CurrentThread.ManagedThreadId);
        }
    }
}