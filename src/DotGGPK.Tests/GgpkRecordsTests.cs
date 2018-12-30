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
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace DotGGPK.Tests
{
    /// <summary>
    /// Contains tests for the <see cref="GgpkRecords"/> class.
    /// </summary>
    [TestClass]
    public class GgpkRecordsTests
    {
        #region Methods

        /// <summary>
        /// Checks if a null reference is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromStringNullTest()
        {
            IEnumerable<GgpkRecord> records = GgpkRecords.From((string)null);
        }

        /// <summary>
        /// Checks if a non existing file is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FromStringNotExistsTest()
        {
            IEnumerable<GgpkRecord> records = GgpkRecords.From(@"C:\i_do_not_exist.test");
        }

        /// <summary>
        /// Checks if a null reference is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromFileNullTest()
        {
            IEnumerable<GgpkRecord> records = GgpkRecords.From((FileInfo)null);
        }

        /// <summary>
        /// Checks if a non existing file is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FromFileNotExistsTest()
        {
            IEnumerable<GgpkRecord> records = GgpkRecords.From(new FileInfo(@"C:\i_do_not_exist.test"));
        }

        /// <summary>
        /// Checks if a file with wrong record marker length is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(GgpkException))]
        public void FromFileWrongLengthTest()
        {
            IEnumerable<GgpkRecord> records = GgpkRecords.From(@"fail-length.ggpk");
        }

        /// <summary>
        /// Checks if a file with wrong record marker type is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(GgpkException))]
        public void FromFileWrongMarkerTest()
        {
            IEnumerable<GgpkRecord> records = GgpkRecords.From(@"fail-marker.ggpk");
        }

        /// <summary>
        /// Checks if a valid ggpk file is read correctly.
        /// </summary>
        [TestMethod]
        public void FromTest()
        {
            IEnumerable<GgpkRecord> records = GgpkRecords.From(@"pass.ggpk");

            Assert.AreEqual<int>(5, records.Count());
            Assert.AreEqual<int>(1, records.OfType<GgpkMainRecord>().Count());
            Assert.AreEqual<int>(1, records.OfType<GgpkDirectoryRecord>().Count());
            Assert.AreEqual<int>(2, records.OfType<GgpkFileRecord>().Count());
            Assert.AreEqual<int>(1, records.OfType<GgpkFreeRecord>().Count());

            GgpkMainRecord mainRecord = records.OfType<GgpkMainRecord>().FirstOrDefault();
            GgpkDirectoryRecord dirRecord = records.OfType<GgpkDirectoryRecord>().FirstOrDefault();
            GgpkFileRecord fileRecord1 = records.OfType<GgpkFileRecord>().Where(r => r.Offset == 20).FirstOrDefault();
            GgpkFileRecord fileRecord2 = records.OfType<GgpkFileRecord>().Where(r => r.Offset == 100).FirstOrDefault();
            GgpkFreeRecord freeRecord = records.OfType<GgpkFreeRecord>().FirstOrDefault();

            Assert.AreEqual<int>(1, mainRecord.RecordOffsets.Count());

            Assert.AreEqual<string>("Dir_1", dirRecord.DirectoryName);
            Assert.AreEqual<int>(2, dirRecord.Entries.Count());

            Assert.AreEqual<string>("test-file-1.bin", fileRecord1.FileName);
            Assert.AreEqual<ulong>(4, fileRecord1.FileLength);

            Assert.AreEqual<string>("Aa_Bb-Cc.DdEe", fileRecord2.FileName);
            Assert.AreEqual<ulong>(6, fileRecord2.FileLength);

            Assert.AreEqual<ulong>(0, freeRecord.NextFreeRecordOffset);

            foreach (GgpkRecord record in records)
            {
                Assert.AreNotEqual<string>(string.Empty, record.ToString());
            }
        }

        #endregion
    }
}
