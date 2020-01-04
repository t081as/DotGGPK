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
using System.Collections.Generic;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Describes directories in a ggpk archive that may contain other
    /// directories or files.
    /// </summary>
    public interface IGgpkDirectory : IGgpkElement
    {
        #region Properties

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <value>An instance of <see cref="IGgpkDirectory"/> or <c>null</c> if this is the root element.</value>
        IGgpkDirectory Parent { get; }

        /// <summary>
        /// Gets the subdirectories in this directory.
        /// </summary>
        /// <value>The subdirectories in this directory.</value>
        IEnumerable<IGgpkDirectory> Directories { get; }

        /// <summary>
        /// Gets the files in this directory.
        /// </summary>
        /// <value>The files in this directory.</value>
        IEnumerable<IGgpkFile> Files { get; }

        #endregion
    }
}
