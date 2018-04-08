using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GzipArchiver.FileExtenders
{
    internal class ChunkedFileWriter : IChunkedFile
    {
        private FileStream _outputStream;
        private long Lenght => _outputStream.Length;

        public void OpenResources(string outputFile)
        {
            _outputStream = File.Open(outputFile, FileMode.OpenOrCreate);
        }

        public void Chunk(int startPos, byte[] chunk)
        {
            lock (_outputStream)
            {
                _outputStream.Seek(startPos, SeekOrigin.Begin);
                _outputStream.Write(chunk, 0, chunk.Length);
            }
        }

        public void Dispose()
        {
            _outputStream.Close();
            GC.SuppressFinalize(this);
        }

        ~ChunkedFileWriter()
        {
            Dispose();
        }
    }
}
