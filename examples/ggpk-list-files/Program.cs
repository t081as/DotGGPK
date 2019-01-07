using System;
using System.IO;
using DotGGPK;

namespace ggpk_list_files
{
    class Program
    {
        static void Main(string[] args)
        {
            GgpkArchive archive = GgpkArchive.From(Path.Combine(Environment.GetEnvironmentVariable("POE_PATH"), "content.ggpk"));
            ListFiles(archive.Root);
        }

        static void ListFiles(IGgpkDirectory currentDirectory)
        {
            foreach (IGgpkFile file in currentDirectory.Files)
            {
                Console.WriteLine(file.FullName);
            }

            foreach (IGgpkDirectory directory in currentDirectory.Directories)
            {
                ListFiles(directory);
            }
        }
    }
}
