namespace GzipArchiver.Args
{
    class ArgsOptions
    {
        public string InputFile { get; private set; }
        public string OuputFile { get; private set; }

        public ArgsOptions(string inputFile, string ouputFile)
        {
            InputFile = inputFile;
            OuputFile = ouputFile;
        }
    }
}
