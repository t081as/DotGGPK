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
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents a single entry in a <see cref="GgpkDirectoryRecord"/>.
    /// </summary>
    public class GgpkDirectoryRecordEntry
    {
        #region Properties

        /// <summary>
        /// Gets or sets the timestamp of the entry.
        /// </summary>
        /// <value>The timestamp of the entry.</value>
        public uint TimeStamp { get; set; } = 0;

        /// <summary>
        /// Gets or sets the offset of the element this entry points to.
        /// </summary>
        /// <value>The offset of the element this entry points to.</value>
        public ulong Offset { get; set; } = 0;

        #endregion
    }
}