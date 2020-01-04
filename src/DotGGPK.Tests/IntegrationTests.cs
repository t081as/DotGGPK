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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace DotGGPK.Tests
{
    /// <summary>
    /// Contains integration tests.
    /// </summary>
    /// <remarks>
    /// The tests defined in this class use the official content.ggpk file and can only be executed if path of exile is installed.
    /// </remarks>
    [TestClass]
    public class IntegrationTests
    {
        #region Methods

        /// <summary>
        /// Checks if the official content.ggpk can be read using the <see cref="GgpkRecords"/> class.
        /// </summary>
        [TestMethod]
        public void TestRecords()
        {
            string poePath = Environment.GetEnvironmentVariable("POE_PATH");

            if (string.IsNullOrEmpty(poePath))
            {
                Assert.Inconclusive("Environment variable POE_PATH not defined - skipping test");
            }

            string contentFile = Path.Combine(poePath, "content.ggpk");

            if (!File.Exists(contentFile))
            {
                Assert.Inconclusive("content.ggpk not found - skipping test");
            }

            IEnumerable<GgpkRecord> records = GgpkRecords.From(contentFile);
            Assert.IsTrue(records.Count() > 0);
        }

        /// <summary>
        /// Checks if the official content.ggpk can be read using the <see cref="GgpkArchive"/> class.
        /// </summary>
        [TestMethod]
        public void TestArchive()
        {
            string poePath = Environment.GetEnvironmentVariable("POE_PATH");

            if (string.IsNullOrEmpty(poePath))
            {
                Assert.Inconclusive("Environment variable POE_PATH not defined - skipping test");
            }

            string contentFile = Path.Combine(poePath, "content.ggpk");

            if (!File.Exists(contentFile))
            {
                Assert.Inconclusive("content.ggpk not found - skipping test");
            }

            GgpkArchive archive = GgpkArchive.From(contentFile);

            Assert.IsNotNull(archive.Root);

            IEnumerable<IGgpkFile> files = archive.Root.ToFileList();

            foreach (var file in files)
            {
                StringAssert.StartsWith(file.FullName, "/");
                Assert.AreEqual(false, file.FullName.Contains('/', StringComparison.InvariantCultureIgnoreCase));
            }

            IGgpkDirectory dialogueDirectory = archive.GetDirectory("/Audio/Dialogue/");
            Assert.IsNotNull(dialogueDirectory);

            IGgpkFile noAudioFoundFile = archive.GetFile("/Audio/NoFileFound.ogg");
            Assert.IsNotNull(noAudioFoundFile);
        }

        #endregion
    }
}
