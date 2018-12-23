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
    /// Contains methods to read a ggpk archive in raw format.
    /// </summary>
    public static class GgpkEntries
    {
        #region Methods

        /// <summary>
        /// Reads the given ggpk archive file and returns all <see cref="GgpkEntry">entries</see>.
        /// </summary>
        /// <param name="fileName">The ggpk archive file.</param>
        /// <returns>All <see cref="GgpkEntry">entries</see> read from the file.</returns>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is <c>null</c>.</exception>
        /// <exception cref="FileNotFoundException"><c>fileName</c> does not exist.</exception>
        public static IEnumerable<GgpkEntry> FromFile(string fileName)
        {
            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            return FromFile(new FileInfo(fileName));
        }

        /// <summary>
        /// Reads the given ggpk archive file and returns all <see cref="GgpkEntry">entries</see>.
        /// </summary>
        /// <param name="file">The ggpk archive file.</param>
        /// <returns>All <see cref="GgpkEntry">entries</see> read from the file.</returns>
        /// <exception cref="ArgumentNullException"><c>file</c> is <c>null</c>.</exception>
        /// <exception cref="FileNotFoundException"><c>file</c> does not exist.</exception>
        public static IEnumerable<GgpkEntry> FromFile(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException($"Archive file {file.FullName} not found", file.FullName);
            }

            List<GgpkEntry> entries = new List<GgpkEntry>();

            using (Stream ggpkStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                while (ggpkStream.Position < ggpkStream.Length)
                {
                    (uint entryMarkerLength, string entryMarkerType) = ReadEntryMarker(ggpkStream);

                    byte[] entryBuffer = new byte[entryMarkerLength - 8]; // uint: 4 bytes, string: 4 bytes have already been read

                    if (ggpkStream.Read(entryBuffer, 0, entryBuffer.Length) != entryBuffer.Length)
                    {
                        throw new InvalidDataException();
                    }

                    MemoryStream entryStream = new MemoryStream(entryBuffer);
                    //// GgpkEntries currentEntry = null;

                    switch (entryMarkerType)
                    {
                        case "GGPK":
                            break;

                        default:
                            throw new InvalidDataException();
                    }
                }
            }

            return entries;
        }

        /// <summary>
        /// Reads a ggpk entry marker in the given <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that shall be read.</param>
        /// <returns>An entry marker consisting of an entry marker length and an entry marker type.</returns>
        private static(uint, string) ReadEntryMarker(Stream stream)
        {
            byte[] binaryEntryLength = new byte[4];
            byte[] binaryEntryType = new byte[4];

            if (stream.Read(binaryEntryLength, 0, binaryEntryLength.Length) != binaryEntryLength.Length)
            {
                throw new InvalidDataException("Unable to read entry marker length");
            }

            if (stream.Read(binaryEntryType, 0, binaryEntryType.Length) != binaryEntryType.Length)
            {
                throw new InvalidDataException("Unable to read entry marker type");
            }

            uint entryLength = BitConverter.ToUInt32(binaryEntryLength, 0);
            string entryType = Encoding.ASCII.GetString(binaryEntryType);

            return (entryLength, entryType);
        }

        #endregion
    }
}