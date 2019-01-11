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
    /// Represents a single file in a ggpk archive.
    /// </summary>
    internal class GgpkFile : GgpkElement, IGgpkFile
    {
        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="IGgpkDirectory">directory.</see>
        /// </summary>
        /// <value>An instance of <see cref="IGgpkDirectory"/> or <c>null</c> if this is the root element.</value>
        public IGgpkDirectory Parent { get; set; } = null;

        /// <summary>
        /// Gets or sets the offset of the actual file data in the ggpk archive file.
        /// </summary>
        /// <value>The offset of the actual file data in the ggpk archive file.</value>
        public ulong Offset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the length of the actual file data, in byte.
        /// </summary>
        /// <value>The length of the actual file data, in byte.</value>
        public ulong Length { get; set; } = 0;

        /// <summary>
        /// Gets the full name of the element.
        /// </summary>
        /// <value>The full name of the element.</value>
        public override string FullName
        {
            get
            {
                return this.GetName(this.Parent, this.Name);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates and returns a <see cref="Stream"/> representing the actual file data.
        /// </summary>
        /// <returns>A <see cref="Stream"/> representing the actual file data.</returns>
        public Stream GetStream()
        {
            return new GgpkStream(
                new FileStream(
                    this.ArchiveFileName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read),
                this.Offset,
                this.Length);
        }

        #endregion
    }
}