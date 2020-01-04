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
using System.IO;
using System.Linq;
using System.Text;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents a <see cref="GgpkRecord"/> containing a directory.
    /// </summary>
    public class GgpkDirectoryRecord : GgpkRecord
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the directory.
        /// </summary>
        /// <value>The name of the directory.</value>
        public string DirectoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SHA-256 hash of the data.
        /// </summary>
        /// <value>The SHA-256 hash of the data.</value>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the entries of the directory.
        /// </summary>
        /// <value>The entries of the directory.</value>
        public IEnumerable<GgpkDirectoryRecordEntry> Entries { get; set; } = new List<GgpkDirectoryRecordEntry>();

        #endregion

        #region Methods

        /// <summary>
        /// Reads a <see cref="GgpkDirectoryRecord"/> from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> that shall be read.</param>
        /// <returns>A <see cref="GgpkDirectoryRecord"/>.</returns>
        public static GgpkDirectoryRecord From(BinaryReader reader)
        {
            uint directoryNameLength = reader.ReadUInt32();
            uint numberOfEntries = reader.ReadUInt32();
            string hash = Convert.ToBase64String(reader.ReadBytes(32));
            string directoryName = Encoding.Unicode.GetString(reader.ReadBytes((int)directoryNameLength * 2)).TrimEnd('\0');

            List<GgpkDirectoryRecordEntry> entries = new List<GgpkDirectoryRecordEntry>();

            for (int i = 0; i < numberOfEntries; i++)
            {
                entries.Add(new GgpkDirectoryRecordEntry()
                {
                    TimeStamp = reader.ReadUInt32(),
                    Offset = reader.ReadUInt64()
                });
            }

            return new GgpkDirectoryRecord()
            {
                DirectoryName = directoryName,
                Hash = hash,
                Entries = entries
            };
        }

        /// <summary>
        /// Gets the <see cref="string"/> representation of this class.
        /// </summary>
        /// <returns>The <see cref="string"/> representation of this class.</returns>
        public override string ToString() =>
            $@"PDIR:
Directory name: {this.DirectoryName}
Hash: {this.Hash}
Elements: {this.Entries.Count()}";

        #endregion
    }
}
