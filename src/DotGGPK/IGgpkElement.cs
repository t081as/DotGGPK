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
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Describes common properties of directories and files.
    /// </summary>
    public interface IGgpkElement
    {
        #region Properties

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        /// <value>The name of the element.</value>
        string Name { get; }

        /// <summary>
        /// Gets the timestamp of the element.
        /// </summary>
        /// <value>The timestamp of the element.</value>
        uint TimeStamp { get; }

        /// <summary>
        /// Gets the SHA-256 hash of the data.
        /// </summary>
        /// <value>The SHA-256 hash of the data.</value>
        string Hash { get; }

        /// <summary>
        /// Gets the full name of the element.
        /// </summary>
        /// <value>The full name of the element.</value>
        string FullName { get; }

        #endregion
    }
}
