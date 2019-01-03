﻿#region MIT License
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
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents a directory in a ggpk archive that may contain other
    /// <see cref="GgpkDirectory">directories</see> or <see cref="GgpkFile">files</see>.
    /// </summary>
    public class GgpkDirectory : GgpkElement
    {
        #region Properties

        /// <summary>
        /// Gets or sets the subdirectories.
        /// </summary>
        public IEnumerable<GgpkDirectory> Directories { get; set; } = new List<GgpkDirectory>();

        /// <summary>
        /// Gets or sets the files.
        /// </summary>
        public IEnumerable<GgpkFile> Files { get; set; } = new List<GgpkFile>();

        #endregion
    }
}
