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
using System.IO;
using System.Text;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents the ggpk record marker that identifies length and type of each ggpk record.
    /// </summary>
    public sealed class GgpkRecordMarker
    {
        #region Constants and Fields

        /// <summary>
        /// The size of a <see cref="GgpkRecordMarker"/>.
        /// </summary>
        /// <value>The size of a <see cref="GgpkRecordMarker"/>, in byte.</value>
        public const int Size = 8;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the offset of this record marker.
        /// </summary>
        /// <value>The offset of this record marker.</value>
        public ulong Offset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the length of the complete record (including this record marker).
        /// </summary>
        /// <value>The length of the complete record (including this record marker).</value>
        public uint Length { get; set; } = 0;

        /// <summary>
        /// Gets or sets the record type following this record marker.
        /// </summary>
        /// <value>The record type following this record marker.</value>
        public string Type { get; set; } = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        /// Reads a <see cref="GgpkRecordMarker"/> from the given <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> that shall be used.</param>
        /// <returns>A <see cref="GgpkRecordMarker"/>.</returns>
        public static GgpkRecordMarker From(BinaryReader reader)
        {
            return new GgpkRecordMarker()
            {
                Offset = (ulong)reader.BaseStream.Position,
                Length = reader.ReadUInt32(),
                Type = Encoding.ASCII.GetString(reader.ReadBytes(4))
            };
        }

        #endregion
    }
}