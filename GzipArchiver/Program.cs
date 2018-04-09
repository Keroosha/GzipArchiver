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
            //todo make help object!
            //todo actualize!
            if (args.Length <= 1)
            {
                Console.WriteLine("Simple gzip archiver");
                Console.WriteLine("Syntax: GzipArchiver -i file -o file.gz");
                Console.WriteLine("-i input file");
                Console.WriteLine("-o output file");
                return;
            }

            var options = new ArgsParser().Parse(args);

            var read = new ChunkedFileReader();

            read.OpenResources(options.InputFile);

            long operated = 0;
            long size = read.Lenght;
            long chunkSize = size / options.Chunks;

            while (operated < size)
            {
                var test = new ChunkedGzipWriter();

                var chunkNumber = (int)((operated / chunkSize) + 1);
                var zeroes = string.Concat(
                    Enumerable.Repeat("0", 3 - chunkNumber.ToString().Length).ToArray()
                );

                test.OpenResources(options.OuputFile + "." + zeroes + chunkNumber);

                if ((operated + chunkSize) > size)
                {
                    var finalChunkSize = size - operated;
                    var finalChunk = read.Chunk((int)operated, (int)finalChunkSize);

                    test.Chunk(0, finalChunk);
                    test.Dispose();
                    break;
                }

                var chunk = read.Chunk((int)operated, (int)chunkSize);
                test.Chunk(0, chunk);
                test.Dispose();

                operated += chunkSize;
                Console.WriteLine("{0} operated of {1}", operated, size);
            }

            read.Dispose();
        }
    }
}
