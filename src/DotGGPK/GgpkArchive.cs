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
    /// Represents a package of files in the ggpk archive format.
    /// </summary>
    public sealed class GgpkArchive
    {
        #region Constants and Fields

        /// <summary>
        /// The exposed root directory of the ggpk archive.
        /// </summary>
        private GgpkDirectory root;

        #endregion

        #region Constrcutors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GgpkArchive"/> class.
        /// </summary>
        /// <param name="fileName">The ggpk archive file.</param>
        /// <exception cref="GgpkException">Error while reading the archive.</exception>
        internal GgpkArchive(string fileName)
        {
            this.root = new GgpkDirectory();

            IEnumerable<GgpkRecord> records = GgpkRecords.From(fileName);
            GgpkMainRecord mainRecord = records.OfType<GgpkMainRecord>().FirstOrDefault();

            if (mainRecord is null)
            {
                throw new GgpkException($"Error while analyzing the ggpk archive file: no record of type GGPK found");
            }

            IEnumerable<GgpkDirectoryRecordEntry> createdEntries = mainRecord.RecordOffsets.Select((offset) => new GgpkDirectoryRecordEntry()
            {
                Offset = offset,
                TimeStamp = 0
            }).ToList();

            BuildDirectoryTree(ref this.root, createdEntries, records);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root directory of the ggpk file.
        /// </summary>
        public GgpkDirectory Root
        {
            get
            {
                return this.root;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the given ggpk archive file and returns all <see cref="GgpkRecord">records</see>.
        /// </summary>
        /// <param name="fileName">The ggpk archive file.</param>
        /// <returns>All <see cref="GgpkRecord">records</see> read from the file.</returns>
        /// <exception cref="ArgumentNullException"><c>fileName</c> is <c>null</c>.</exception>
        /// <exception cref="FileNotFoundException"><c>fileName</c> does not exist.</exception>
        /// <exception cref="GgpkException">Error while reading the archive.</exception>
        public static GgpkArchive From(string fileName)
        {
            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"Archive file {fileName} not found", fileName);
            }

            return new GgpkArchive(fileName);
        }

        /// <summary>
        /// Reads the given ggpk archive file and returns all <see cref="GgpkRecord">records</see>.
        /// </summary>
        /// <param name="file">The ggpk archive file.</param>
        /// <returns>All <see cref="GgpkRecord">records</see> read from the file.</returns>
        /// <exception cref="ArgumentNullException"><c>file</c> is <c>null</c>.</exception>
        /// <exception cref="FileNotFoundException"><c>file</c> does not exist.</exception>
        /// <exception cref="GgpkException">Error while reading the archive.</exception>
        public static GgpkArchive From(FileInfo file)
        {
            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!file.Exists)
            {
                throw new FileNotFoundException($"Archive file {file.FullName} not found", file.FullName);
            }

            return From(file.FullName);
        }

        /// <summary>
        /// Builds the directory and file tree.
        /// </summary>
        /// <param name="currentDirectory">The current directory.</param>
        /// <param name="currentDirectoryContent">The desired content of the current directory.</param>
        /// <param name="records">All <see cref="GgpkRecord">records</see> from the ggpk archive used to find the references.</param>
        private static void BuildDirectoryTree(
            ref GgpkDirectory currentDirectory,
            IEnumerable<GgpkDirectoryRecordEntry> currentDirectoryContent,
            IEnumerable<GgpkRecord> records)
        {
            foreach (GgpkDirectoryRecordEntry recordEntry in currentDirectoryContent)
            {
                GgpkRecord referencedRecord = records.Where(r => r.Offset == recordEntry.Offset).FirstOrDefault();

                if (referencedRecord is null)
                {
                    throw new GgpkException($"Error while analyzing the ggpk archive file: no element found at offset {recordEntry.Offset}");
                }

                switch (referencedRecord)
                {
                    case GgpkDirectoryRecord directoryRecord:

                        GgpkDirectory directory = new GgpkDirectory()
                        {
                            Name = directoryRecord.DirectoryName,
                            TimeStamp = recordEntry.TimeStamp
                        };

                        currentDirectory.Add(directory);
                        BuildDirectoryTree(ref directory, directoryRecord.Entries, records);

                        break;

                    case GgpkFileRecord fileRecord:

                        GgpkFile file = new GgpkFile()
                        {
                            Name = fileRecord.FileName,
                            TimeStamp = recordEntry.TimeStamp
                        };

                        currentDirectory.Add(file);

                        break;

                    default:
                        throw new GgpkException($"Error while analyzing the ggpk archive file: no element of type directory or file found at offset {recordEntry.Offset}");
                }
            }
        }

        #endregion
    }
}