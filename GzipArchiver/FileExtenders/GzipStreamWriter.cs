using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;

namespace GzipArchiver.FileExtenders
{
    /// <summary>
    /// GZip stream writer -
    /// only sequential write,
    /// do not forget to Dispose object!
    /// </summary>
    internal class ChunkedGzipWriter : IChunkedFile, IChunkedFileWriter
    {
        private GZipStream _outputStream;
        private long Lenght => _outputStream.Length;

        public void OpenResources(string outputFile)
        {
            _outputStream = new GZipStream(File.Open(outputFile, FileMode.OpenOrCreate), CompressionMode.Compress);
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

        ~ChunkedGzipWriter()
        {
            Dispose();
        }
    }
}
