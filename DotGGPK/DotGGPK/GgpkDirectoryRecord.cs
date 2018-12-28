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
using System.Text;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents the a ggpk record containing a directory.
    /// </summary>
    public class GgpkDirectoryRecord : GgpkRecord
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the directory.
        /// </summary>
        public string DirectoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SHA-256 hash of the data.
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the entries of this <see cref="GgpkDirectoryRecord"/>.
        /// </summary>
        public IEnumerable<GgpkDirectoryRecordEntry> Entries { get; set; } = new List<GgpkDirectoryRecordEntry>();

        #endregion

        #region Methods

        /// <summary>
        /// Reads a <see cref="GgpkDirectoryRecord"/> from the given <see cref="BinaryReader"/>.
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

        #endregion
    }
}
