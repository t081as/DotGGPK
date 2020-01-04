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
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Contains methods for reading a ggpk archive in raw format.
    /// </summary>
    /// <remarks>
    /// The returned <see cref="IEnumerable{T}"/> contains the following record types:
    /// <list type="bullet">
    /// <item>
    /// <term><see cref="GgpkMainRecord"/></term>
    /// </item>
    /// <item>
    /// <term><see cref="GgpkFileRecord"/></term>
    /// </item>
    /// <item>
    /// <term><see cref="GgpkDirectoryRecord"/></term>
    /// </item>
    /// <item>
    /// <term><see cref="GgpkFreeRecord"/></term>
    /// </item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following example demonstrates how to use the <see cref="GgpkRecords"/> class to read a ggpk archive:
    /// <code>
    /// IEnumerable&lt;GgpkRecord&gt; records = GgpkRecords.From("/path/to/content.ggpk");
    ///
    /// foreach (GgpkRecord record in records)
    /// {
    ///     Console.WriteLine($"Record: {record.GetType()} @ offset {record.Offset} (length: {record.Length})");
    /// }
    /// </code>
    /// </example>
    public static class GgpkRecords
    {
        #region Methods

        /// <summary>
        /// Reads the given ggpk archive file and returns all records.
        /// </summary>
        /// <param name="fileName">The ggpk archive file.</param>
        /// <returns>All records read from the file.</returns>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is <c>null</c>.</exception>
        /// <exception cref="FileNotFoundException"><c>fileName</c> does not exist.</exception>
        /// <exception cref="GgpkException">Error while reading the archive.</exception>
        public static IEnumerable<GgpkRecord> From(string fileName)
        {
            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            return From(new FileInfo(fileName));
        }

        /// <summary>
        /// Reads the given ggpk archive file and returns all records.
        /// </summary>
        /// <param name="file">The ggpk archive file.</param>
        /// <returns>All records read from the file.</returns>
        /// <exception cref="ArgumentNullException"><c>file</c> is <c>null</c>.</exception>
        /// <exception cref="FileNotFoundException"><c>file</c> does not exist.</exception>
        /// <exception cref="GgpkException">Error while reading the archive.</exception>
        public static IEnumerable<GgpkRecord> From(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException($"Archive file {file.FullName} not found", file.FullName);
            }

            return From(new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        /// <summary>
        /// Reads the given ggpk archive <paramref name="stream"/> and returns all records.
        /// </summary>
        /// <param name="stream">The ggpk <see cref="Stream"/>.</param>
        /// <returns>All records read from the <see cref="Stream"/>.</returns>
        /// <exception cref="ArgumentNullException"><c>stream</c> is <c>null</c>.</exception>
        /// <exception cref="GgpkException">Error while reading the archive.</exception>
        public static IEnumerable<GgpkRecord> From(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            List<GgpkRecord> records = new List<GgpkRecord>();

            using (BinaryReader ggpkStreamReader = new BinaryReader(stream))
            {
                try
                {
                    while (ggpkStreamReader.BaseStream.Position < ggpkStreamReader.BaseStream.Length)
                    {
                        GgpkRecordMarker recordMarker = GgpkRecordMarker.From(ggpkStreamReader);
                        GgpkRecord currentRecord = null;

                        if (recordMarker.Offset + recordMarker.Length > (ulong)ggpkStreamReader.BaseStream.Length)
                        {
                            throw new InvalidDataException($"Invalid record length {recordMarker.Length} at offset {recordMarker.Offset}");
                        }

                        switch (recordMarker.Type)
                        {
                            case "GGPK":
                                currentRecord = GgpkMainRecord.From(ggpkStreamReader);
                                break;

                            case "FREE":
                                currentRecord = GgpkFreeRecord.From(recordMarker, ggpkStreamReader);
                                break;

                            case "FILE":
                                currentRecord = GgpkFileRecord.From(recordMarker, ggpkStreamReader);
                                break;

                            case "PDIR":
                                currentRecord = GgpkDirectoryRecord.From(ggpkStreamReader);
                                break;

                            default:
                                throw new InvalidDataException($"Unknown record type {recordMarker.Type} at offset {recordMarker.Offset}");
                        }

                        currentRecord.Offset = recordMarker.Offset;
                        currentRecord.Length = recordMarker.Length;

                        records.Add(currentRecord);
                    }
                }
                catch (Exception ex)
                {
                    throw new GgpkException($"Error while parsing the ggpk archive file", ex)
                    {
                        Offset = ggpkStreamReader.BaseStream.Position
                    };
                }
            }

            return records;
        }

        #endregion
    }
}