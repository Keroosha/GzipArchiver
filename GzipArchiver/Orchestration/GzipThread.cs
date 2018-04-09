using System;
using System.Collections.Generic;
using System.Threading;
using GzipArchiver.FileExtenders;

namespace GzipArchiver.Orchestration
{
    internal class GzipThread
    {
        private const int SleepTime = 5;

        private readonly Queue<GzipJob> _gzipJobs;
        private bool _status;

        public event EventHandler JobDone;

        public GzipThread(Queue<GzipJob> gzipJobs)
        {
            _gzipJobs = gzipJobs;
            _status = true;
        }

        public void StopHandler()
        {
            _status = false;
        }

        private void JobScan()
        {
            while (_status)
            {
                GzipJob job;
                lock (_gzipJobs)
                {
                    if (_gzipJobs.Count > 0)
                    {
                        job = _gzipJobs.Dequeue();
                    }
                    else
                    {
                        continue;
                    }
                }
                CompressJob(job);
            }
        }

        private void CompressJob(GzipJob gzipJob)
        {
            try
            {
                using (var chunkedGzip = new ChunkedGzipWriter())
                {
                    chunkedGzip.OpenResources(gzipJob.ChunkFile);
                    chunkedGzip.Chunk(0, gzipJob.Chunks);
                }

                OnJobDone();
            }
            catch (Exception E)
            {

            }

        }

        protected virtual void OnJobDone()
        {
            JobDone?.Invoke(this, EventArgs.Empty);
        }
    }
}