using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using GzipArchiver.Args;
using GzipArchiver.FileExtenders;

namespace GzipArchiver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 1)
            {
                Console.WriteLine("Simple gzip archiver");
                Console.WriteLine("Syntax: GzipArchiver -i file -o file.gz");
                Console.WriteLine("-i input file");
                Console.WriteLine("-o output file");
                return;
            }

            var options = new ArgsParser().Parse(args);

            var test = new ChunkedGzipWriter();
            var read = new ChunkedFileReader();

            read.OpenResources(options.InputFile);
            test.OpenResources(options.OuputFile);

            long operated = 0;
            long size = read.Lenght;
            long chunkSize = size / 2;

            while (operated < size)
            {
                if ((operated + chunkSize) > size)
                {
                    var finalChunkSize = size - operated;
                    var finalChunk = read.Chunk((int)operated, (int)finalChunkSize);
                    test.Chunk((int)operated, finalChunk);
                    break;
                }

                var chunk = read.Chunk((int)operated, (int)chunkSize);
                test.Chunk((int)operated, chunk);
                operated += chunkSize;
            }

            read.Dispose();
            test.Dispose();
        }
    }
}
