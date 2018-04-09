using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GzipArchiver.Orchestration
{
    class GzipThreaded<TGzipThread> where TGzipThread:IGzipThread
    {
        private Thread[] _threads;
        private readonly TGzipThread _thread;

        public bool Status
        {
            get
            {
                var status = true;
                for (var index = 0; index < _threads.Length; index++)
                {
                    status &= _threads[index].IsAlive;
                }

                return status;
            }
        }

        public GzipThreaded(TGzipThread thread, int threadCount, ParameterizedThreadStart threadRoutine)
        {
            _thread = thread;
            _threads = new Thread[threadCount];
            for (var index = 0; index < _threads.Length; index++)
            {
                _threads[index] = new Thread(threadRoutine);
            }
        }

        public void Execute()
        {
            for (var index = 0; index < _threads.Length; index++)
            {
                _threads[index].Start(_thread);
            }
        }
    }
}
