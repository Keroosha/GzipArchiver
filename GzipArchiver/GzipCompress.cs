using System;
using System.Collections.Generic;
using System.Linq;
using GzipArchiver.Args;
using GzipArchiver.FileExtenders;
using GzipArchiver.Orchestration;

namespace GzipArchiver
{
    public class GzipCompress : IDisposable
    {
        private readonly ArchiverOptions _archiverOptions;
        private readonly ChunkedFileReader _chunkedFileReader;
        private readonly Queue<GzipJob> _gzipJobs;

        private readonly long _chunkSize;

        public long Operated { get; private set; }
        public long Size => _chunkedFileReader.Lenght;

        public GzipCompress(ArchiverOptions archiverOptions)
        {
            _archiverOptions = archiverOptions ?? throw new ArgumentNullException(nameof(archiverOptions));
            _chunkedFileReader = new ChunkedFileReader();
            _chunkedFileReader.OpenResources(_archiverOptions.InputFile);
            _chunkSize = Size / _archiverOptions.Chunks;
            Operated = 0;
        }

        public void Compress()
        {
            while (Operated < Size)
            {
                using (var chunkedGzipWriter = new ChunkedGzipWriter())
                {   
                    var chunkNumber = (int)((Operated / _chunkSize) + 1);
                    var zeroes = string.Concat(
                        Enumerable.Repeat("0", 3 - chunkNumber.ToString().Length).ToArray()
                    );

                    chunkedGzipWriter.OpenResources(_archiverOptions.OuputFile + "." + zeroes + chunkNumber);

                    if ((Operated + _chunkSize) > Size)
                    {
                        var finalChunkSize = Size - Operated;
                        var finalChunk = _chunkedFileReader.Chunk((int)Operated, (int)finalChunkSize);

                        chunkedGzipWriter.Chunk(0, finalChunk);
                        break;
                    }

                    var chunk = _chunkedFileReader.Chunk((int)Operated, (int)_chunkSize);
                    chunkedGzipWriter.Chunk(0, chunk);

                    Operated += _chunkSize;
                    Console.WriteLine("{0} Operated of {1}", Operated, Size);
                }
            }
        }


        public void Dispose()
        {
            _chunkedFileReader.Dispose();
        }
    }
}
