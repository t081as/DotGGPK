using System;
using System.Collections.Generic;
using System.IO;
using DotGGPK;

namespace ggpk_list_files
{
    class Program
    {
        static void Main(string[] args)
        {
            GgpkArchive archive = GgpkArchive.From(Path.Combine(Environment.GetEnvironmentVariable("POE_PATH"), "content.ggpk"));
            IEnumerable<IGgpkFile> allFiles = archive.Root.ToFileList();
            
            foreach (var file in allFiles)
            {
                Console.WriteLine(file.FullName);
            }
        }
    }
}
