using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GzipArchiver.Args;
using GzipArchiver.FileExtenders;

namespace GzipArchiver.Orchestration
{
    class GzipTask
    {
        private readonly ArchiverOptions _options;
        private readonly ChunkedFileReader _fileReader;
    }
}
