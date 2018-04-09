using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GzipArchiver.Args;
using GzipArchiver.FileExtenders;
using GzipArchiver.Orchestration;

namespace GzipArchiver
{
    public class GzipCompress : IDisposable
    {
        private readonly ArchiverOptions _archiverOptions;
        private readonly ChunkedFileReader _chunkedFileReader;
        private readonly Queue<GzipCompressJob> _gzipJobs;

        private readonly long _chunkSize;
        private const int ThreadTimeout = 100;

        public long Operated { get; private set; }
        public long Size => _chunkedFileReader.Lenght;

        public GzipCompress(ArchiverOptions archiverOptions)
        {
            _archiverOptions = archiverOptions ?? throw new ArgumentNullException(nameof(archiverOptions));
            _chunkedFileReader = new ChunkedFileReader();
            _chunkedFileReader.OpenResources(_archiverOptions.InputFile);
            _chunkSize = Size / _archiverOptions.Chunks;
            _gzipJobs = new Queue<GzipCompressJob>((int)_archiverOptions.JobsCount);
            Operated = 0;
        }

        public void Compress()
        {
            long readed = 0;

            var threadWorker = new GzipCompressThread(_gzipJobs);
            threadWorker.JobDone += ThreadWorker_JobDone;

            var threads = new GzipThreaded<GzipCompressThread>(
                threadWorker,
                (int)_archiverOptions.ThreadCount,
                Start
                );

            threads.Execute();
            

            while (Operated < Size)
            {
                if (_gzipJobs.Count < _archiverOptions.JobsCount && readed < Size)
                {
                    lock (_gzipJobs)
                    {
                        readed = GenJob(readed);
                    }
                }
            }

            threadWorker.StopHandler();
            threadWorker.JobDone -= ThreadWorker_JobDone;

            while (threads.Status)
            {
                Console.WriteLine("Waiting for thread closing");
                Thread.Sleep(ThreadTimeout);
            }
        }

        private void ThreadWorker_JobDone(object sender, EventArgs e)
        {
            Operated += _chunkSize;
            Console.WriteLine("{0} Operated of {1}", Operated, Size);
        }

        private void Start(object o)
        {
            var jober = o as GzipCompressThread;
            jober.JobScan();
        }

        private long GenJob(long readed)
        {
            var chunkNumber = (int)((readed / _chunkSize) + 1);
            var zeroes = string.Concat(
                Enumerable.Repeat("0", 3 - chunkNumber.ToString().Length).ToArray()
            );

            if ((readed + _chunkSize) > Size)
            {
                var finalChunkSize = Size - readed;
                var finalChunk = _chunkedFileReader.Chunk((int)readed, (int)finalChunkSize);
                _gzipJobs.Enqueue(new GzipCompressJob
                {
                    Chunks = finalChunk,
                    ChunkFile = _archiverOptions.OuputFile + "." + zeroes + chunkNumber
                });
                return readed + _chunkSize;
            }
            var chunk = _chunkedFileReader.Chunk((int)readed, (int)_chunkSize);

            _gzipJobs.Enqueue(new GzipCompressJob
            {
                Chunks = chunk,
                ChunkFile = _archiverOptions.OuputFile + "." + zeroes + chunkNumber
            });
            return readed + _chunkSize;
        }


        public void Dispose()
        {
            _chunkedFileReader.Dispose();
        }
    }
}
