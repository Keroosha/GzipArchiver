using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace GzipArchiver.FileExtenders
{
    class ChunkedMemWriter
    {
        private readonly GZipStream _outputStream;
        private readonly MemoryStream _memory;
        public long Lenght => _outputStream.Length;

        public ChunkedMemWriter()
        {
            _memory = new MemoryStream();
            _outputStream = new GZipStream(_memory, CompressionMode.Compress);
        }

        public byte[] Chunk(int startPos, byte[] chunk)
        {
            lock (_outputStream)
            {
                _outputStream.Write(chunk, 0, chunk.Length);
            }

            return _memory.ToArray();
        }

        public byte[] GetGzipedData()
        {
            return _memory.ToArray();
        }

        public void Dispose()
        {
            _outputStream.Close();
            _memory.Close();
            GC.SuppressFinalize(this);
        }

        ~ChunkedMemWriter()
        {
            Dispose();
        }
    }
}
