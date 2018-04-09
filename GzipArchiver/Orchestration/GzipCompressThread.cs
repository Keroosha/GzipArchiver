using System;
using System.Collections.Generic;
using GzipArchiver.FileExtenders;

namespace GzipArchiver.Orchestration
{
    public class GzipCompressThread : IGzipThread
    {
        private readonly Queue<GzipCompressJob> _gzipJobs;
        private bool _status;

        public event EventHandler JobDone;

        public GzipCompressThread(Queue<GzipCompressJob> gzipJobs)
        {
            _gzipJobs = gzipJobs;
            _status = true;
        }

        public void StopHandler()
        {
            _status = false;
        }

        public void JobScan()
        {
            while (_status)
            {
                GzipCompressJob compressJob;
                lock (_gzipJobs)
                {
                    if (_gzipJobs.Count > 0)
                    {
                        compressJob = _gzipJobs.Dequeue();
                    }
                    else
                    {
                        continue;
                    }
                }
                CompressJob(compressJob);
            }
        }

        private void CompressJob(GzipCompressJob gzipCompressJob)
        {
            try
            {
                using (var chunkedGzip = new ChunkedGzipWriter())
                {
                    chunkedGzip.OpenResources(gzipCompressJob.ChunkFile);
                    chunkedGzip.Chunk(0, gzipCompressJob.Chunks);
                }

                OnJobDone();
            }
            catch (Exception E)
            {
                //TODO error handler
            }

        }

        protected virtual void OnJobDone()
        {
            JobDone?.Invoke(this, EventArgs.Empty);
        }
    }
}