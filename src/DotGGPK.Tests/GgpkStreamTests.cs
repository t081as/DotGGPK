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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace DotGGPK.Tests
{
    /// <summary>
    /// Contains tests for the <see cref="GgpkStreamTests"/> class.
    /// </summary>
    [TestClass]
    public class GgpkStreamTests
    {
        #region Constants and Fields

        /// <summary>
        /// Represents the <see cref="GgpkArchive"/> used for the unit tests.
        /// </summary>
        private GgpkArchive archive;

        /// <summary>
        /// The <see cref="IGgpkFile"/> used for the unit tests.
        /// </summary>
        private IGgpkFile file;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GgpkStreamTests"/> class.
        /// </summary>
        public GgpkStreamTests()
        {
            this.archive = GgpkArchive.From(@"pass.ggpk");
            this.file = this.archive.Root.Directories.Where(d => d.Name == "Dir_1").FirstOrDefault().Files.Where(f => f.Name == "Aa_Bb-Cc.DdEe").FirstOrDefault();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks the stream properties.
        /// </summary>
        [TestMethod]
        public void PropertiesTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                Assert.AreEqual(true, stream.CanRead);
                Assert.AreEqual(true, stream.CanSeek);
                Assert.AreEqual(false, stream.CanWrite);
                Assert.AreEqual(6, stream.Length);
            }
        }

        /// <summary>
        /// Checks seeking in the stream.
        /// </summary>
        [TestMethod]
        public void SeekBeginTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                Assert.AreEqual(0, stream.Position);
                Assert.AreEqual(0, stream.Seek(0, SeekOrigin.Begin));
                Assert.AreEqual(0, stream.Position);
            }
        }

        /// <summary>
        /// Checks seeking in the stream.
        /// </summary>
        [TestMethod]
        public void SeekEndTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                Assert.AreEqual(0, stream.Position);
                Assert.AreEqual((long)this.file.Length - 1, stream.Seek(-1, SeekOrigin.End));
                Assert.AreEqual((long)this.file.Length - 1, stream.Position);
            }
        }

        /// <summary>
        /// Checks seeking in the stream.
        /// </summary>
        [TestMethod]
        public void SeekCurrentTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                Assert.AreEqual(0, stream.Position);
                Assert.AreEqual(1, stream.Seek(1, SeekOrigin.Current));
                Assert.AreEqual(3, stream.Seek(2, SeekOrigin.Current));
                Assert.AreEqual(3, stream.Position);
            }
        }

        /// <summary>
        /// Checks seeking in the stream.
        /// </summary>
        [TestMethod]
        public void SeekCurrentLowerTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                Assert.AreEqual(0, stream.Position);
                Assert.AreEqual(0, stream.Seek(-500, SeekOrigin.Current));
                Assert.AreEqual(0, stream.Position);
            }
        }

        /// <summary>
        /// Checks seeking in the stream.
        /// </summary>
        [TestMethod]
        public void PositionTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                stream.Position = 2;
                Assert.AreEqual(2, stream.Position);
            }
        }

        /// <summary>
        /// Checks reading the stream.
        /// </summary>
        [TestMethod]
        public void ReadAllTest()
        {
            using (BinaryReader reader = new BinaryReader(this.file.GetStream()))
            {
                byte[] data = reader.ReadBytes((int)reader.BaseStream.Length);

                Assert.AreEqual(6, data.Length);

                Assert.AreEqual(4, data[0]);
                Assert.AreEqual(5, data[1]);
                Assert.AreEqual(3, data[2]);
                Assert.AreEqual(9, data[3]);
                Assert.AreEqual(50, data[4]);
                Assert.AreEqual(254, data[5]);
            }
        }

        /// <summary>
        /// Checks reading the stream.
        /// </summary>
        [TestMethod]
        public void ReadPartTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                byte[] data = new byte[2];

                stream.Position = 2;
                Assert.AreEqual(2, stream.Read(data, 0, 2));

                Assert.AreEqual(2, data.Length);

                Assert.AreEqual(3, data[0]);
                Assert.AreEqual(9, data[1]);
            }
        }

        /// <summary>
        /// Checks reading the stream.
        /// </summary>
        [TestMethod]
        public void ReadEndTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                byte[] data = new byte[2];

                stream.Position = stream.Length;
                Assert.AreEqual(0, stream.Read(data, 0, 2));
            }
        }

        /// <summary>
        /// Checks write access to the stream.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void FlushTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                stream.Flush();
            }
        }

        /// <summary>
        /// Checks write access to the stream.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SetLengthTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                stream.SetLength(1);
            }
        }

        /// <summary>
        /// Checks write access to the stream.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void WriteTest()
        {
            using (Stream stream = this.file.GetStream())
            {
                stream.Write(new byte[] { 2, 1 }, 0, 2);
            }
        }

        #endregion
    }
}
