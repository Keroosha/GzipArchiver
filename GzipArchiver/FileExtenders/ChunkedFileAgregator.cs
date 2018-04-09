using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace GzipArchiver.FileExtenders
{
    class ChunkedFileAgregator : IChunkedFile, IChunkedFileWriter, IDisposable
    {
        private GZipStream _outputStream;
        public long Lenght => _outputStream.Length;

        public void OpenResources(string outputFile)
        {
            _outputStream = new GZipStream(
                    File.Open(outputFile, FileMode.OpenOrCreate),
                    CompressionMode.Compress
                );
        }

        public void Chunk(int startPos, byte[] chunk)
        {
            lock (_outputStream)
            {
                _outputStream.Write(chunk, 0, chunk.Length);
            }
        }

        public void Dispose()
        {
            _outputStream.Close();
            GC.SuppressFinalize(this);
        }

        ~ChunkedFileAgregator()
        {
            Dispose();
        }
    }
}
