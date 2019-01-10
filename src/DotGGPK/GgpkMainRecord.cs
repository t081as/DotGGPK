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
using System.Collections.Generic;
using System.IO;
using System.Linq;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents the main <see cref="GgpkRecord"/> in a ggpk archive file.
    /// </summary>
    public class GgpkMainRecord : GgpkRecord
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list with offsets of PDIR and FREE records.
        /// </summary>
        /// <value>The list with offsets of records.</value>
        public IEnumerable<ulong> RecordOffsets { get; set; } = new List<ulong>();

        #endregion

        #region Methods

        /// <summary>
        /// Reads a <see cref="GgpkMainRecord"/> from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> that shall be read.</param>
        /// <returns>A <see cref="GgpkMainRecord"/>.</returns>
        public static GgpkMainRecord From(BinaryReader reader)
        {
            List<ulong> recordOffsets = new List<ulong>();

            uint numberOfRecords = reader.ReadUInt32();

            for (int i = 0; i < numberOfRecords; i++)
            {
                recordOffsets.Add(reader.ReadUInt64());
            }

            return new GgpkMainRecord()
            {
                RecordOffsets = recordOffsets
            };
        }

        /// <summary>
        /// Gets the <see cref="string"/> representation of this class.
        /// </summary>
        /// <returns>The <see cref="string"/> representation of this class.</returns>
        public override string ToString() =>
$@"GGPK:
Elements: {this.RecordOffsets.Count()}";

        #endregion
    }
}