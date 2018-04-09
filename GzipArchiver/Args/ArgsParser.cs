using System;

namespace GzipArchiver.Args
{
    public class ArgsParser
    {
        private string[] _argsStrings;

        /// <summary>
        /// Generate from Arg array Archive settings
        /// </summary>
        /// <param name="argsStrings">Args array from Main function</param>
        /// <returns>Archiver Options object</returns>
        public ArchiverOptions Parse(string[] argsStrings)
        {
            _argsStrings = argsStrings;
            return new ArchiverOptions(
                FindValue("-i"),
                FindValue("-o"),
                Convert.ToInt64(FindValue("-c")),
                Convert.ToInt64(FindValue("-t")),
                Convert.ToInt64(FindValue("-j"))
                );
        }

        /// <summary>
        /// Try to find value from args
        /// </summary>
        /// <param name="key">option key with '-' on start</param>
        /// <returns>param from args</returns>
        private string FindValue(string key)
        {
            try
            {
                return _argsStrings[
                    Array.FindIndex(_argsStrings, value => value == key) + 1
                ];
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
