using System;
using System.Collections.Generic;
using System.IO;
using DotGGPK;

namespace ggpk_read_raw
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<GgpkRecord> records = GgpkRecords.From(Path.Combine(Environment.GetEnvironmentVariable("POE_PATH"), "content.ggpk"));

            foreach (GgpkRecord record in records)
            {
                Console.WriteLine($"Record: {record.GetType()} @ offset {record.Offset} (length: {record.Length})");
            }
        }
    }
}