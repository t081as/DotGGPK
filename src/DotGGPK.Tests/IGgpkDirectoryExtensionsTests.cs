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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endregion

namespace DotGGPK.Tests
{
    /// <summary>
    /// Contains tests for the <see cref="IGgpkDirectoryExtensions"/> class.
    /// </summary>
    [TestClass]
    public class IGgpkDirectoryExtensionsTests
    {
        #region Methods

        /// <summary>
        /// Checks the <see cref="IGgpkDirectoryExtensions.ToFileList(IGgpkDirectory)"/> method.
        /// </summary>
        [TestMethod]
        public void ToFileListTest()
        {
            GgpkArchive archive = GgpkArchive.From(@"pass.ggpk");
            IEnumerable<IGgpkFile> allFiles = archive.Root.ToFileList();

            Assert.AreEqual(2, allFiles.Count());
            Assert.IsNotNull(allFiles.Where(f => f.Name == "test-file-1.bin").FirstOrDefault());
            Assert.IsNotNull(allFiles.Where(f => f.Name == "Aa_Bb-Cc.DdEe").FirstOrDefault());
        }

        #endregion
    }
}
