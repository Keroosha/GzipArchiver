using System;

namespace GzipArchiver.Args
{
    /// <summary>
    /// Options to archiver
    /// </summary>
    class ArchiverOptions
    {
        /// <summary>
        /// Read from this file
        /// </summary>
        public string InputFile { get;  set; }
        /// <summary>
        /// Write to this
        /// </summary>
        public string OuputFile { get;  set; }
        /// <summary>
        /// How many chunks to create
        /// </summary>
        public long Chunks { get;  set; }
        /// <summary>
        /// Compressing threads
        /// </summary>
        public long ThreadCount { get;  set; }
        /// <summary>
        /// Job pool count
        /// </summary>
        public long JobsCount { get;  set; }

        public ArchiverOptions(string inputFile, string ouputFile, long chunks, long threadCount, long jobsCount)
        {
            InputFile = inputFile ?? throw new ArgumentException("No input file specified");
            OuputFile = ouputFile ?? inputFile + ".gz";
            Chunks = Chunks = !(chunks <= 999) ? throw new ArgumentException("999 max chunks size")
                : (chunks < 1) ? 1 : chunks;
            ThreadCount = threadCount < 1 ? 1 : threadCount;
            JobsCount = jobsCount < 1 ? 1 : jobsCount;
        }
    }
}
