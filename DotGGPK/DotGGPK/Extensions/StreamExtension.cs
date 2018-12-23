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
#endregion

namespace DotGGPK.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="Stream"/> class.
    /// </summary>
    public static class StreamExtension
    {
        #region Methods

        /// <summary>
        /// Reads the given number of bytes from the given stream and returns a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> that shall be read.</param>
        /// <param name="length">The number of bytes that shall be read.</param>
        /// <returns>A <see cref="MemoryStream"/> containing the data.</returns>
        /// <exception cref="InvalidOperationException">Unable to read the given number of bytes.</exception>
        public static MemoryStream ReadToMemoryStream(this Stream stream, int length)
        {
            byte[] data = new byte[length];

            if (stream.Read(data, 0, (int)length) != length)
            {
                throw new InvalidOperationException($"UJnable to read {length} byte(s) from the stream");
            }

            return new MemoryStream(data);
        }

        #endregion
    }
}
