using System;

namespace GzipArchiver.Args
{
    class ArgsParser
    {
        public ArgsOptions Parse(string[] argsStrings)
        {
            var paths = FindPathStrings(argsStrings);
            return new ArgsOptions(paths[0], paths[1]);
        }

        private string[] FindPathStrings(string[] argsStrings)
        {

            var inputPos = Array.FindIndex(argsStrings, value => value == "-i");
            var outputPos = Array.FindIndex(argsStrings, value => value == "-o");

            return new string[2]
            {
                argsStrings[inputPos + 1],
                argsStrings[outputPos + 1]
            };
        }
    }
}
