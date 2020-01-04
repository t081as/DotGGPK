#region MIT License
// The MIT License (MIT)
//
// Copyright © 2018-2020 Tobias Koch <t.koch@tk-software.de>
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region Namespaces
using System;
using System.Collections.Generic;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Contains extension methods for the <see cref="IGgpkDirectory"/> interface.
    /// </summary>
    public static class IGgpkDirectoryExtensions
    {
        #region Methods

        /// <summary>
        /// Returns the files of the given directory and all subdirectories.
        /// </summary>
        /// <param name="directory">The given directory.</param>
        /// <returns>All files inside the given directory and all subdirectories.</returns>
        public static IEnumerable<IGgpkFile> ToFileList(this IGgpkDirectory directory)
        {
            List<IGgpkFile> files = new List<IGgpkFile>();
            files.AddRange(directory.Files);

            foreach (IGgpkDirectory subDirectory in directory.Directories)
            {
                files.AddRange(subDirectory.ToFileList());
            }

            return files;
        }

        #endregion
    }
}
