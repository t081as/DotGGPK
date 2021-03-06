﻿#region MIT License
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
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents a package of files in the ggpk archive format.
    /// </summary>
    /// <example>
    /// The following example demonstrates how to use the <see cref="GgpkArchive"/> class to read a ggpk archive:
    /// <code>
    /// GgpkArchive archive = GgpkArchive.From("/path/to/content.ggpk");
    ///
    /// foreach (var file in archive.Root.ToFileList())
    /// {
    ///     Console.WriteLine(file.FullName);
    /// }
    /// </code>
    /// </example>
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
            IEnumerable<GgpkRecord> records = GgpkRecords.From(fileName);
            GgpkMainRecord mainRecord = records.OfType<GgpkMainRecord>().FirstOrDefault();

            this.RawRecords = records;
            this.root = new GgpkDirectory();
            this.FileName = fileName;

            if (mainRecord is null)
            {
                throw new GgpkException($"Error while analyzing the ggpk archive file: no record of type GGPK found");
            }

            // Convert references in main record (ulong) to type GgpkDirectoryRecordEntry required by BuildDirectoryTree-method
            IEnumerable<GgpkDirectoryRecordEntry> createdEntries = mainRecord.RecordOffsets.Select((offset) => new GgpkDirectoryRecordEntry()
            {
                Offset = offset,
                TimeStamp = 0
            }).ToList();

            // Create record dictionary (which is /much/ faster than querying the IEnumerable with Linq)
            Dictionary<ulong, GgpkRecord> recordDictionary = new Dictionary<ulong, GgpkRecord>();

            foreach (GgpkRecord record in records)
            {
                recordDictionary.Add(record.Offset, record);
            }

            this.BuildDirectoryTree(null, createdEntries, recordDictionary);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root directory of the ggpk file.
        /// </summary>
        /// <value>The root directory of the ggpk file.</value>
        /// <remarks>
        /// This is the main entry point when working with a ggpk archive file. It contains all folders
        /// and files of the archive.
        /// </remarks>
        /// <example>
        /// The following example demonstrates how to use the <see cref="GgpkArchive.Root"/> property
        /// to access the file system of a ggpk archive:
        /// <code>
        /// using System;
        /// using System.IO;
        /// using DotGGPK;
        ///
        /// namespace ggpk_root
        /// {
        ///     class Program
        ///     {
        ///         static void Main(string[] args)
        ///         {
        ///             GgpkArchive archive = GgpkArchive.From("/path/to/content.ggpk");
        ///             PrintDirectory(archive.Root);
        ///         }
        ///
        ///         static void PrintDirectory(IGgpkDirectory directory)
        ///         {
        ///             foreach (var file in directory.Files)
        ///             {
        ///                 Console.WriteLine(file.FullName);
        ///             }
        ///
        ///             foreach (var subDirectory in directory.Directories)
        ///             {
        ///                 PrintDirectory(subDirectory);
        ///             }
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        public IGgpkDirectory Root
        {
            get
            {
                return this.root;
            }
        }

        /// <summary>
        /// Gets the underlaying raw ggpk records.
        /// </summary>
        /// <value>The underlaying raw ggpk records.</value>
        /// <remarks>
        /// Additional information about the different record types can be found here: <see cref="GgpkRecords"/>
        /// </remarks>
        public IEnumerable<GgpkRecord> RawRecords { get; private set; }

        /// <summary>
        /// Gets the file name of the ggpk archive file.
        /// </summary>
        /// <value>The file name of the ggpk archive file.</value>
        public string FileName { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the given ggpk archive file and returns a <see cref="GgpkArchive"/>.
        /// </summary>
        /// <param name="fileName">The ggpk archive file.</param>
        /// <returns>A <see cref="GgpkArchive"/>.</returns>
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
        /// Reads the given ggpk archive file and returns a <see cref="GgpkArchive"/>.
        /// </summary>
        /// <param name="file">The ggpk archive file.</param>
        /// <returns>A <see cref="GgpkArchive"/>.</returns>
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
        /// Gets an <see cref="IGgpkDirectory"/> based on the given path.
        /// </summary>
        /// <param name="path">The path of the directory to search for.</param>
        /// <returns>An <see cref="IGgpkDirectory"/> representing the directory.</returns>
        /// <exception cref="DirectoryNotFoundException"><c>path</c> does not exist.</exception>
        /// <example>
        /// The following example demonstrates how to use <see cref="GgpkArchive.GetDirectory(string)"/> method:
        /// <code>
        /// GgpkArchive archive = GgpkArchive.From("/path/to/content.ggpk");
        /// IGgpkDirectory dialogueDirectory = archive.GetDirectory("/Audio/Dialogue/");
        /// </code>
        /// </example>
        public IGgpkDirectory GetDirectory(string path)
        {
            string[] pathParts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            IGgpkDirectory currentDirectory = this.Root;

            foreach (string pathPart in pathParts)
            {
                currentDirectory = currentDirectory.Directories.Where(d => d.Name == pathPart).FirstOrDefault();

                if (currentDirectory is null)
                {
                    throw new DirectoryNotFoundException($"Path {path} not found");
                }
            }

            return currentDirectory;
        }

        /// <summary>
        /// Gets an <see cref="IGgpkFile"/> based on the given path.
        /// </summary>
        /// <param name="path">The path of the file to search for.</param>
        /// <returns>An <see cref="IGgpkFile"/> representing the file.</returns>
        /// <exception cref="FileNotFoundException"><c>path</c> does not exist.</exception>
        /// <example>
        /// The following example demonstrates how to use <see cref="GgpkArchive.GetFile(string)"/> method:
        /// <code>
        /// GgpkArchive archive = GgpkArchive.From("/path/to/content.ggpk");
        /// IGgpkFile noAudioFoundFile = archive.GetFile("/Audio/NoFileFound.ogg");
        /// </code>
        /// </example>
        public IGgpkFile GetFile(string path)
        {
            string[] pathParts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            IGgpkDirectory currentDirectory = this.Root;

            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                currentDirectory = currentDirectory.Directories.Where(d => d.Name == pathParts[i]).FirstOrDefault();

                if (currentDirectory is null)
                {
                    throw new FileNotFoundException($"File {path} not found");
                }
            }

            IGgpkFile file = currentDirectory.Files.Where(f => f.Name == pathParts[pathParts.Length - 1]).FirstOrDefault();

            if (file is null)
            {
                throw new FileNotFoundException($"File {path} not found");
            }

            return file;
        }

        /// <summary>
        /// Builds the directory and file tree.
        /// </summary>
        /// <param name="currentDirectory">The current directory.</param>
        /// <param name="currentDirectoryContent">The desired content of the current directory.</param>
        /// <param name="records">All <see cref="GgpkRecord">records</see> from the ggpk archive used to find the references.</param>
        private void BuildDirectoryTree(
            GgpkDirectory currentDirectory,
            IEnumerable<GgpkDirectoryRecordEntry> currentDirectoryContent,
            IDictionary<ulong, GgpkRecord> records)
        {
            foreach (GgpkDirectoryRecordEntry recordEntry in currentDirectoryContent)
            {
                GgpkRecord referencedRecord = records[recordEntry.Offset];

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
                            TimeStamp = recordEntry.TimeStamp,
                            Hash = directoryRecord.Hash,
                            Parent = currentDirectory,
                            ArchiveFileName = this.FileName
                        };

                        if (currentDirectory != null)
                        {
                            currentDirectory.Add(directory);
                        }
                        else
                        {
                            this.root = directory;
                        }

                        this.BuildDirectoryTree(directory, directoryRecord.Entries, records);

                        break;

                    case GgpkFileRecord fileRecord:

                        GgpkFile file = new GgpkFile()
                        {
                            Name = fileRecord.FileName,
                            TimeStamp = recordEntry.TimeStamp,
                            Hash = fileRecord.Hash,
                            Offset = fileRecord.FileOffset,
                            Length = fileRecord.FileLength,
                            Parent = currentDirectory,
                            ArchiveFileName = this.FileName
                        };

                        currentDirectory.Add(file);

                        break;

                    case GgpkFreeRecord freeRecord:
                        break;

                    default:
                        throw new GgpkException($"Error while analyzing the ggpk archive file: no element of type directory or file found at offset {recordEntry.Offset}");
                }
            }
        }

        #endregion
    }
}