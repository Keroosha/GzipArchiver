using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GzipArchiver.FileExtenders
{
    internal class ChunkedFileReader : IDisposable, IChunkedFileReader, IChunkedFile
    {
        private FileStream _inputStream;
        public long Lenght => _inputStream.Length;

        public void OpenResources(string outputFile)
        {
            if (!File.Exists(outputFile))
            {
                throw new FileNotFoundException("File not found", outputFile);
            }

            _inputStream = File.Open(outputFile, FileMode.Open);
        }

        public byte[] Chunk(int startPos, int size)
        {
            byte[] chunk = new byte[size];

            lock (_inputStream)
            {
                _inputStream.Seek(startPos, SeekOrigin.Begin);
                _inputStream.Read(chunk, 0, chunk.Length);
            }
            return chunk;
        }

        public void Dispose()
        {
            _inputStream.Close();
            GC.SuppressFinalize(this);
        }

        ~ChunkedFileReader()
        {
            Dispose();
        }
    }
}
