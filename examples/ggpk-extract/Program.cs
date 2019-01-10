using DotGGPK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ggpk_extract
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceFile = Path.Combine(Environment.GetEnvironmentVariable("POE_PATH"), "content.ggpk");
            string destinationDirectory = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, "content-ggpk");

            GgpkArchive archive = GgpkArchive.From(sourceFile);
            IEnumerable<IGgpkFile> files = archive.Root.ToFileList();

            foreach (var file in files)
            {
                string destinationFileName = Path.Combine(destinationDirectory, file.FullName.Replace('/', '\\').Substring(1));

                Directory.CreateDirectory(new FileInfo(destinationFileName).DirectoryName);

                using (BinaryWriter destinationFileWriter = new BinaryWriter(File.Create(destinationFileName)))
                {
                    using (BinaryReader sourceFileReader = new BinaryReader(file.GetStream()))
                    {
                        Console.WriteLine($"Extracting {file.FullName}...");

                        destinationFileWriter.Write(sourceFileReader.ReadBytes((int)file.Length));
                        destinationFileWriter.Flush();
                    }
                }
            }
        }
    }
}
