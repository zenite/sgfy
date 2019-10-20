
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace javascripttest.entity
{
    public class ThreadWorkers
    {
        public ThreadWorkers(ThreadStart start)
        {
            thread = new Thread(start);
            index = 0;
        }

        public void Start()
        {
            thread.Start();
        }
        public bool stop { get; set; }
        public int index { get; set; }
        private Thread thread;

    }
}
