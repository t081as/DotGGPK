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
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace DotGGPK.Tests
{
    /// <summary>
    /// Contains tests for the <see cref="GgpkArchive"/> class.
    /// </summary>
    [TestClass]
    public class GgpkArchiveTests
    {
        #region Methods

        /// <summary>
        /// Checks if a null reference is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromStringNullTest()
        {
            GgpkArchive archive = GgpkArchive.From((string)null);
        }

        /// <summary>
        /// Checks if a non existing file is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FromStringNotExistsTest()
        {
            GgpkArchive archive = GgpkArchive.From(@"C:\i_do_not_exist.test");
        }

        /// <summary>
        /// Checks if a null reference is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromFileNullTest()
        {
            GgpkArchive archive = GgpkArchive.From((FileInfo)null);
        }

        /// <summary>
        /// Checks if a non existing file is detected correctly.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void FromFileNotExistsTest()
        {
            GgpkArchive archive = GgpkArchive.From(new FileInfo(@"C:\i_do_not_exist.test"));
        }

        /// <summary>
        /// Checks if a valid ggpk file is read correctly.
        /// </summary>
        [TestMethod]
        public void FromTest()
        {
            GgpkArchive archive = GgpkArchive.From(new FileInfo(@"pass.ggpk"));

            Assert.AreEqual<int>(1, archive.Root.Directories.Count());

            IGgpkDirectory dir1 = archive.Root.Directories.FirstOrDefault();

            Assert.AreEqual("Dir_1", dir1.Name);

            IGgpkFile file1 = dir1.Files.Where(f => f.Name == "test-file-1.bin").FirstOrDefault();
            IGgpkFile file2 = dir1.Files.Where(f => f.Name == "Aa_Bb-Cc.DdEe").FirstOrDefault();

            Assert.IsNotNull(file1);
            Assert.IsNotNull(file2);

            Assert.AreEqual<ulong>(4, file1.Length);

            Assert.IsNull(archive.Root.Parent);
            Assert.IsNotNull(dir1.Parent);
            Assert.IsNotNull(file1.Parent);
            Assert.IsNotNull(file2.Parent);
        }

        /// <summary>
        /// Checks <see cref="GgpkArchive.GetFile(string)"/> method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetFileDirectoryNotFoundTest()
        {
            GgpkArchive archive = GgpkArchive.From(new FileInfo(@"pass.ggpk"));
            IGgpkFile file = archive.GetFile("/NonExistingDirectory/test-file-1.bin");
        }

        /// <summary>
        /// Checks the <see cref="GgpkArchive.GetFile(string)"/> method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetFileNotFoundTest()
        {
            GgpkArchive archive = GgpkArchive.From(new FileInfo(@"pass.ggpk"));
            IGgpkFile file = archive.GetFile("/Dir_1/i_do_not_exist.non");
        }

        /// <summary>
        /// Checks the <see cref="GgpkArchive.GetFile(string)"/> method.
        /// </summary>
        /// <param name="path">The path of the file.</param>
        [DataTestMethod]
        [DataRow("/Dir_1/test-file-1.bin")]
        [DataRow("Dir_1/test-file-1.bin")]
        public void GetFileTest(string path)
        {
            GgpkArchive archive = GgpkArchive.From(new FileInfo(@"pass.ggpk"));
            IGgpkFile file = archive.GetFile(path);

            Assert.AreEqual(4UL, file.Length);
        }

        /// <summary>
        /// Checks the <see cref="GgpkArchive.GetDirectory(string)"/> method.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetDirectoryNotFoundTest()
        {
            GgpkArchive archive = GgpkArchive.From(new FileInfo(@"pass.ggpk"));
            IGgpkDirectory dir = archive.GetDirectory("/NonExistingDirectory/");
        }

        /// <summary>
        /// Checks the <see cref="GgpkArchive.GetDirectory(string)"/> method.
        /// </summary>
        /// <param name="path">The path of the directory.</param>
        [DataTestMethod]
        [DataRow("Dir_1")]
        [DataRow("Dir_1/")]
        [DataRow("/Dir_1")]
        [DataRow("/Dir_1/")]
        public void GetDirectoryTest(string path)
        {
            GgpkArchive archive = GgpkArchive.From(new FileInfo(@"pass.ggpk"));
            IGgpkDirectory dir = archive.GetDirectory(path);

            Assert.AreEqual(2, dir.Files.Count());
        }

        #endregion
    }
}
