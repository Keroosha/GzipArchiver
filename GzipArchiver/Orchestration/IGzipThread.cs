using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GzipArchiver.Orchestration
{
    interface IGzipThread
    {
        void JobScan();
        void StopHandler();
    }
}
