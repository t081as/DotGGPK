using System;
using System.IO;
using DotGGPK;

namespace ggpk_root
{
    class Program
    {
        static void Main(string[] args)
        {
            GgpkArchive archive = GgpkArchive.From(Path.Combine(Environment.GetEnvironmentVariable("POE_PATH"), "content.ggpk"));
            PrintDirectory(archive.Root);
        }

        static void PrintDirectory(IGgpkDirectory directory)
        {
            foreach (var file in directory.Files)
            {
                Console.WriteLine(file.FullName);
            }

            foreach (var subDirectory in directory.Directories)
            {
                PrintDirectory(subDirectory);
            }
        }

    }
}
