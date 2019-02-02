using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using net.ericsonj.thread;

namespace ConsoleApp1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            ThreadPoolManager pool = new ThreadPoolManager(2);
            MyTask task1 = new MyTask(5000);
            MyTask task2 = new MyTask(5000);
            MyTask task3 = new MyTask(5000);
            pool.execute(task1);
            pool.execute(task2);
            
            pool.JoinThreads();
            
            pool.execute(task3);

            pool.JoinThreads();

            foreach (Thread th in pool.GetThreadList())
            {
                Console.WriteLine(th.IsAlive);
                Console.WriteLine("ThreadState: {0}", th.ThreadState);
            }

            Console.WriteLine("End Thread.");

            //while (true)
            //{
            //    Console.WriteLine(" Thread {0} available ", pool.getThreadCount());
            //    Thread.Sleep(1000);
            //}

        }

    }

    class MyTask : ThreadPoolManager.Runnable
    {
        private int sleep;

        public MyTask(int sleep)
        {
            this.sleep = sleep;
        }

        public override void run()
        {
            Console.WriteLine("Thread {0} start", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(this.sleep);
            Console.WriteLine("Thread {0} end", Thread.CurrentThread.ManagedThreadId);
        }
 
    }
}
