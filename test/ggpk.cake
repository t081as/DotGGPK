using System.IO;
using System.Text;

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("ggpk-fail-length")
    .Does(() =>
{
    using (BinaryWriter writer = new BinaryWriter(System.IO.File.Create("fail-length.ggpk")))
    {
        writer.Write((uint)uint.MaxValue);
        writer.Write(Encoding.ASCII.GetBytes("GGPK"));
        writer.Write((uint)0);
    }
});

Task("ggpk-fail-marker")
    .Does(() =>
{
    using (BinaryWriter writer = new BinaryWriter(System.IO.File.Create("fail-marker.ggpk")))
    {
        writer.Write((uint)12);
        writer.Write(Encoding.ASCII.GetBytes("ABCD"));
        writer.Write((uint)0);
    }
});

Task("ggpk-pass")
    .Does(() =>
{
    using (BinaryWriter writer = new BinaryWriter(System.IO.File.Create("pass.ggpk")))
    {
        /*
         * GGPK Record
         */
        
        // Offset: 0
        writer.Write((uint)20);

        // Offset: 4
        writer.Write(Encoding.ASCII.GetBytes("GGPK"));

        // Offset: 8
        writer.Write((uint)1);

        // Offset: 12
        writer.Write((ulong)178);

        /*
         * FILE Record
         */

        // Offset: 20
        writer.Write((uint)80);

        // Offset: 24
        writer.Write(Encoding.ASCII.GetBytes("FILE"));

        // Offset: 28
        writer.Write((uint)16);

        // Offset: 32
        writer.Write(new byte[32]);

        // Offset: 64
        writer.Write(Encoding.Unicode.GetBytes("test-file-1.bin\0"));

        // Offset: 96
        writer.Write(new byte[] { 0, 1, 2, 3 });

        /*
         * FILE Record
         */

        // Offset: 100
        writer.Write((uint)78);

        // Offset: 104
        writer.Write(Encoding.ASCII.GetBytes("FILE"));

        // Offset: 108
        writer.Write((uint)14);

        // Offset: 112
        writer.Write(new byte[32]);

        // Offset: 144
        writer.Write(Encoding.Unicode.GetBytes("Aa_Bb-Cc.DdEe\0"));

        // Offset: 172
        writer.Write(new byte[] { 4, 5, 3, 9, 50, 254 });

        /*
         * DIRECTORY Record
         */

        // Offset: 178
        writer.Write((uint)84);

        // Offset: 182
        writer.Write(Encoding.ASCII.GetBytes("PDIR"));

        // Offset: 186
        writer.Write((uint)6);

        // Offset: 190
        writer.Write((uint)2);

        // Offset: 194
        writer.Write(new byte[32]);

        // Offset: 226
        writer.Write(Encoding.Unicode.GetBytes("Dir_1\0"));

        // Offset: 238
        writer.Write((uint)0);

        // Offset: 242
        writer.Write((ulong)20);

        // Offset: 250
        writer.Write((uint)120);

        // Offset: 254
        writer.Write((ulong)100);

        /*
         * FREE Record
         */

        // Offset: 262
        writer.Write((uint)18);

        // Offset: 266
        writer.Write(Encoding.ASCII.GetBytes("FREE"));

        // Offset: 270
        writer.Write((ulong)0);

        // Offset: 278
        writer.Write(new byte[2]);

        // Offset: 280
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("ggpk-fail-length")
    .IsDependentOn("ggpk-fail-marker")
    .IsDependentOn("ggpk-pass");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);