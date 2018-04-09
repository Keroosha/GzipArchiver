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

            using (var Gzip = new GzipCompress(options))
            {
                Gzip.Compress();
            }
        }
    }
}
