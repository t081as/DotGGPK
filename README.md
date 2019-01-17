![DotGGPK](https://gitlab.com/tobiaskoch/DotGGPK/raw/master/img/DotGGPK.png)

# DOTGGPK

[![pipeline status](https://gitlab.com/tobiaskoch/DotGGPK/badges/master/pipeline.svg)](https://gitlab.com/tobiaskoch/DotGGPK/commits/master)
[![coverage report](https://gitlab.com/tobiaskoch/DotGGPK/badges/master/coverage.svg)](https://gitlab.com/tobiaskoch/DotGGPK/commits/master)
[![maintained: yes](https://tobiaskoch.gitlab.io/badges/maintained-yes.svg)](https://gitlab.com/tobiaskoch/DotGGPK/commits/master)
[![donate: paypal](https://tobiaskoch.gitlab.io/badges/donate-paypal.svg)](https://www.tk-software.de/donate)

DotGGPK is a .NET standard library for parsing Path of Exile's GGPK archive file format.

## Installation

### Option 0: NuGet
NuGet packages are available [here](https://www.nuget.org/packages/DotGGPK/).

### Option 1: Source
#### Requirements
The following tools must be available:

* [.NET Core SDK](https://dotnet.microsoft.com/download)
* [.NET 4.7.2 Dev Pack](https://dotnet.microsoft.com/download)
* [.NET 4.7.1 Dev Pack](https://dotnet.microsoft.com/download)
* [.NET 4.7 Dev Pack](https://dotnet.microsoft.com/download)
* [.NET 4.6.2 Dev Pack](https://dotnet.microsoft.com/download)
* [.NET 4.6.1 Dev Pack](https://dotnet.microsoft.com/download)
* [.NET 4.6 Dev Pack](https://dotnet.microsoft.com/download)

#### Source code
Get the source code using the following command:

    > git clone https://gitlab.com/tobiaskoch/DotGGPK.git

#### Test
    > ./build.ps1

The script will report if the tests succeeded; the coverage report will be located in the directory *./src/DotGGPK.Tests/bin/Debug/Coverage/*.

#### Build
    > ./build.ps1 --configuration=Release

The libraries will be located in the directory *./src/DotGGPK/bin/Release* if the build succeeds.

## Usage
The documentation of this library can be found here: [https://tobiaskoch.gitlab.io/DotGGPK/](https://tobiaskoch.gitlab.io/DotGGPK/)

## Contributing
see [CONTRIBUTING.md](https://gitlab.com/tobiaskoch/DotGGPK/blob/master/CONTRIBUTING.md)

## Contributors
see [AUTHORS.txt](https://gitlab.com/tobiaskoch/DotGGPK/blob/master/AUTHORS.txt)

## Donating
Thanks for your interest in this project. You can show your appreciation and support further development by [donating](https://www.tk-software.de/donate).

## License
**DotGGPK** © 2018-2019  [Tobias Koch](https://www.tk-software.de). Released under the [MIT License](https://gitlab.com/tobiaskoch/DotGGPK/blob/master/LICENSE.md).
