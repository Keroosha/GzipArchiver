using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GzipArchiver.FileExtenders
{
    interface IChunkedFile
    {
        void OpenResources(string outputFile);
    }
}
