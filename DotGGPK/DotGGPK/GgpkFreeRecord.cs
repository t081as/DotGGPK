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
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents the a ggpk record containing free space.
    /// </summary>
    public class GgpkFreeRecord : GgpkRecord
    {
        #region Methods

        /// <summary>
        /// Reads a <see cref="GgpkFreeRecord"/> from the given <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="marker">The <see cref="GgpkRecordMarker"/> of the record.</param>
        /// <param name="reader">The <see cref="BinaryReader"/> that shall be read.</param>
        /// <returns>A <see cref="GgpkFreeRecord"/>.</returns>
        public static GgpkFreeRecord From(GgpkRecordMarker marker, BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
