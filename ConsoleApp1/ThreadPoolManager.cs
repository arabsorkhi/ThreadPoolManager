using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace net.ericsonj.thread
{

    class ThreadPoolManager
    {
        
        private static Mutex mut = new Mutex();
        private int threadCount;
        private List<Thread> threadList = new List<Thread>();
        private int maxThreads;
        
        public ThreadPoolManager(int threadCount)
        {
            this.threadCount = threadCount;
            this.maxThreads = threadCount;    
        }

        public void execute(Runnable runnable)
        {
            
            List<Thread> threadListRemove = new List<Thread>();
            foreach (Thread objThread in this.threadList)
            {    
                if(!objThread.IsAlive)
                {
                    threadListRemove.Add(objThread);
                }
            }

            foreach (Thread threadToRemove in threadListRemove)
            {
                this.threadList.Remove(threadToRemove);
            }

            if (threadList.Count >= maxThreads)
            {   
                return;
            }   

            runnable.SetPoolManager(this);
            Thread th = new Thread(new ThreadStart(runnable.executeInThread));
            threadList.Add(th);
            th.Start();
        }

        public void JoinThreads()
        {
            foreach (Thread th in threadList)
            {
                th.Join();
            }
        }

        public List<Thread> GetThreadList()
        {
            return threadList;
        }

        public int getThreadCount()
        {
            return threadCount;
        }

        public void DiscountThreadCount()
        {
            this.threadCount--;
        }

        public void addThreadCount()
        {
            this.threadCount++;
        }

        public abstract class Runnable
        {
            private ThreadPoolManager pool;

            public void executeInThread()
            {
                mut.WaitOne();
                pool.DiscountThreadCount();
                mut.ReleaseMutex();
                run();
                mut.WaitOne();
                pool.addThreadCount();
                mut.ReleaseMutex();
            }

            public void SetPoolManager(ThreadPoolManager pool)
            {
                this.pool = pool;
            }

            public abstract void run();

        }

    }

}
