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
        writer.Write((uint)28);

        // Offset: 4
        writer.Write(Encoding.ASCII.GetBytes("GGPK"));

        // Offset: 8
        writer.Write((uint)2);

        // Offset: 12
        writer.Write((ulong)288); // Offset: root PDIR

        // Offset: 20
        writer.Write((ulong)270); // Offset: first FREE

        /*
         * FILE Record
         */

        // Offset: 28
        writer.Write((uint)80);

        // Offset: 32
        writer.Write(Encoding.ASCII.GetBytes("FILE"));

        // Offset: 36
        writer.Write((uint)16);

        // Offset: 40
        writer.Write(new byte[32]);

        // Offset: 72
        writer.Write(Encoding.Unicode.GetBytes("test-file-1.bin\0"));

        // Offset: 104
        writer.Write(new byte[] { 0, 1, 2, 3 });

        /*
         * FILE Record
         */

        // Offset: 108
        writer.Write((uint)78);

        // Offset: 112
        writer.Write(Encoding.ASCII.GetBytes("FILE"));

        // Offset: 116
        writer.Write((uint)14);

        // Offset: 120
        writer.Write(new byte[32]);

        // Offset: 152
        writer.Write(Encoding.Unicode.GetBytes("Aa_Bb-Cc.DdEe\0"));

        // Offset: 180
        writer.Write(new byte[] { 4, 5, 3, 9, 50, 254 });

        /*
         * DIRECTORY Record
         */

        // Offset: 186
        writer.Write((uint)84);

        // Offset: 190
        writer.Write(Encoding.ASCII.GetBytes("PDIR"));

        // Offset: 194
        writer.Write((uint)6);

        // Offset: 198
        writer.Write((uint)2);

        // Offset: 202
        writer.Write(new byte[32]);

        // Offset: 232
        writer.Write(Encoding.Unicode.GetBytes("Dir_1\0"));

        // Offset: 246
        writer.Write((uint)0);

        // Offset: 250
        writer.Write((ulong)28);

        // Offset: 258
        writer.Write((uint)120);

        // Offset: 262
        writer.Write((ulong)108);

        /*
         * FREE Record
         */

        // Offset: 270
        writer.Write((uint)18);

        // Offset: 274
        writer.Write(Encoding.ASCII.GetBytes("FREE"));

        // Offset: 278
        writer.Write((ulong)0);

        // Offset: 286
        writer.Write(new byte[2]);

        /*
         * DIRECTORY Record
         */

        // Offset: 288
        writer.Write((uint)62);

        // Offset: 292
        writer.Write(Encoding.ASCII.GetBytes("PDIR"));

        // Offset: 296
        writer.Write((uint)1);

        // Offset: 300
        writer.Write((uint)1);

        // Offset: 304
        writer.Write(new byte[32]);

        // Offset: 336
        writer.Write(Encoding.Unicode.GetBytes("\0"));

        // Offset: 338
        writer.Write((uint)0);

        // Offset: 342
        writer.Write((ulong)186); // Reference: directory

        // Offset: 350
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