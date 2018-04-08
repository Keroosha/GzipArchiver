using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GzipArchiver.FileExtenders
{
    interface IChunkedFileReader
    {
        byte[] Chunk(int startPos, int size);
    }
}
