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
using System.Text;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents a <see cref="GgpkRecord"/> containing a single file.
    /// </summary>
    public class GgpkFileRecord : GgpkRecord
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SHA-256 hash of the data.
        /// </summary>
        /// <value>The SHA-256 hash of the data.</value>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the offset of the actual file data.
        /// </summary>
        /// <value>The offset of the actual file data.</value>
        public ulong FileOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the length of the actual file data.
        /// </summary>
        /// <value>The length of the actual file data.</value>
        public ulong FileLength { get; set; } = 0;

        #endregion

        #region Methods

        /// <summary>
        /// Reads a <see cref="GgpkFileRecord"/> from the given <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="marker">The <see cref="GgpkRecordMarker"/> of the record.</param>
        /// <param name="reader">The <see cref="BinaryReader"/> that shall be read.</param>
        /// <returns>A <see cref="GgpkFileRecord"/>.</returns>
        public static GgpkFileRecord From(GgpkRecordMarker marker, BinaryReader reader)
        {
            uint fileNameLength = reader.ReadUInt32();
            string hash = Convert.ToBase64String(reader.ReadBytes(32));
            string fileName = Encoding.Unicode.GetString(reader.ReadBytes((int)fileNameLength * 2)).TrimEnd('\0');
            ulong fileOffset = (ulong)reader.BaseStream.Position;
            ulong fileRecordHeaderLength = fileOffset - marker.Offset;
            ulong fileLength = marker.Length - fileRecordHeaderLength;

            GgpkFileRecord record = new GgpkFileRecord
            {
                FileName = fileName,
                Hash = hash,
                FileOffset = fileOffset,
                FileLength = fileLength
            };

            // Skip actual file data, move to position of next record
            reader.BaseStream.Seek((long)record.FileLength, SeekOrigin.Current);

            return record;
        }

        /// <summary>
        /// Gets the <see cref="string"/> representation of this class.
        /// </summary>
        /// <returns>The <see cref="string"/> representation of this class.</returns>
        public override string ToString() =>
            $@"FILE:
File name: {this.FileName}
Hash: {this.Hash}
File offset: {this.FileOffset}
File length: {this.FileLength}";

        #endregion
    }
}
