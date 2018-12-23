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
using DotGGPK.Extensions;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Contains methods for reading a ggpk archive in raw format.
    /// </summary>
    public static class GgpkRecords
    {
        #region Methods

        /// <summary>
        /// Reads the given ggpk archive file and returns all <see cref="GgpkRecord">records</see>.
        /// </summary>
        /// <param name="fileName">The ggpk archive file.</param>
        /// <returns>All <see cref="GgpkRecord">records</see> read from the file.</returns>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is <c>null</c>.</exception>
        /// <exception cref="FileNotFoundException"><c>fileName</c> does not exist.</exception>
        public static IEnumerable<GgpkRecord> FromFile(string fileName)
        {
            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            return FromFile(new FileInfo(fileName));
        }

        /// <summary>
        /// Reads the given ggpk archive file and returns all <see cref="GgpkRecord">records</see>.
        /// </summary>
        /// <param name="file">The ggpk archive file.</param>
        /// <returns>All <see cref="GgpkRecord">records</see> read from the file.</returns>
        /// <exception cref="ArgumentNullException"><c>file</c> is <c>null</c>.</exception>
        /// <exception cref="FileNotFoundException"><c>file</c> does not exist.</exception>
        public static IEnumerable<GgpkRecord> FromFile(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException($"Archive file {file.FullName} not found", file.FullName);
            }

            List<GgpkRecord> records = new List<GgpkRecord>();

            using (Stream ggpkStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    while (ggpkStream.Position < ggpkStream.Length)
                    {
                        long offset = ggpkStream.Position;

                        MemoryStream recordMarkerStream = ggpkStream.Read(GgpkRecordMarker.Size);
                        GgpkRecordMarker recordMarker = GgpkRecordMarker.FromStream(recordMarkerStream);
                        MemoryStream recordStream = ggpkStream.Read((int)recordMarker.Length - GgpkRecordMarker.Size);

                        GgpkRecord currentEntry = null;

                        switch (recordMarker.Type)
                        {
                            case "GGPK":
                                break;

                            default:
                                throw new InvalidDataException($"Unknown record type: {recordMarker.Type}");
                        }

                        currentEntry.Offset = offset;
                        currentEntry.Length = recordMarker.Length - GgpkRecordMarker.Size;

                        records.Add(currentEntry);
                    }
                }
                catch (Exception ex)
                {
                    throw new GgpkException($"Error while parsing archive file {file.FullName}", ex)
                    {
                        FileName = file.FullName,
                        Offset = ggpkStream.Position
                    };
                }
            }

            return records;
        }

        #endregion
    }
}