#region MIT License
// The MIT License (MIT)
//
// Copyright © 2018-2019 Tobias Koch <t.koch@tk-software.de>
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
using System.IO;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Describes single files in a ggpk archive.
    /// </summary>
    public interface IGgpkFile : IGgpkElement
    {
        #region Properties

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        /// <value>An instance of <see cref="IGgpkDirectory"/>.</value>
        IGgpkDirectory Parent { get; }

        /// <summary>
        /// Gets the length of the actual file data.
        /// </summary>
        /// <value>The length of the actual file data, in byte.</value>
        ulong Length { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> representing the actual file data.
        /// </summary>
        /// <returns>A <see cref="Stream"/> representing the actual file data.</returns>
        Stream GetStream();

        #endregion
    }
}
