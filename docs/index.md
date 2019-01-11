# DotGGPK
DotGGPK is a .NET standard library (supporting .NET Framework 4.6+ and .NET Core 1.0+) for parsing Path of Exile's GGPK archive file format.

## Nuget packages
Please find nuget packages of this library here: [https://www.nuget.org/packages/DotGGPK](https://www.nuget.org/packages/DotGGPK/)

## Repository
Please find the source code of this library here: [https://gitlab.com/tobiaskoch/DotGGPK](https://gitlab.com/tobiaskoch/DotGGPK)

## Examples
Please find example code here:

* Example 1: [Read a ggpk archive file in raw mode](https://gitlab.com/tobiaskoch/DotGGPK/tree/master/examples/ggpk-read-raw)
* Example 2: [List all files in a ggpk archive file](https://gitlab.com/tobiaskoch/DotGGPK/tree/master/examples/ggpk-list-files)
* Example 3: [Extract all files in a ggpk archive file](https://gitlab.com/tobiaskoch/DotGGPK/tree/master/examples/ggpk-extract)

## Quick Start Notes:
1. Open a shell and create a new directory

    > mkdir ggpk-cli

    > cd ggpk-cli

2. Create a new console project

    > dotnet new console

3. Add a reference to the DotGGPK package

    > dotnet add package DotGGPK

4. Open the file *Program.cs* and add the following code

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using DotGGPK;

namespace ggpk_cli
{
    class Program
    {
        static void Main(string[] args)
        {
            GgpkArchive archive = GgpkArchive.From(args[0]);
            IEnumerable<IGgpkFile> files = archive.Root.ToFileList();

            Console.WriteLine($"The ggpk archive file '{args[0]}' contains {files.Count()} file(s).");
        }
    }
}
```

5. Run the application

    > dotnet run C:\games\poe\content.ggpk