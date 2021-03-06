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
using System.IO;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents a <see cref="Stream"/> pointing to file data of a single file in a ggpk file.
    /// </summary>
    internal class GgpkStream : Stream
    {
        #region Constants and Fields

        /// <summary>
        /// The text for exceptions thrown if a write operation is triggered.
        /// </summary>
        private const string NotSupported = "Ggpk stream does not support write operations";

        /// <summary>
        /// The underlaying stream representing the ggpk file.
        /// </summary>
        private Stream ggpkStream;

        /// <summary>
        /// The offset of the file data in the ggpk file.
        /// </summary>
        private ulong offset;

        /// <summary>
        /// The length of the file data in the ggpk file.
        /// </summary>
        private ulong length;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GgpkStream"/> class
        /// </summary>
        /// <param name="ggpkStream">The underlaying stream representing the ggpk file.</param>
        /// <param name="offset">The offset of the file data in the ggpk file.</param>
        /// <param name="length">The length of the file data in the ggpk file.</param>
        public GgpkStream(Stream ggpkStream, ulong offset, ulong length)
        {
            this.ggpkStream = ggpkStream;
            this.offset = offset;
            this.length = length;

            this.ggpkStream.Seek((long)this.offset, SeekOrigin.Begin);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value>A value indicating whether the current stream supports reading.</value>
        public override bool CanRead => true;

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value>A value indicating whether the current stream supports seeking.</value>
        public override bool CanSeek => true;

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value>A value indicating whether the current stream supports writing.</value>
        public override bool CanWrite => false;

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        /// <value>The length in bytes of the stream.</value>
        public override long Length => (long)this.length;

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        /// <value>The position within the current stream.</value>
        public override long Position
        {
            get => this.ggpkStream.Position - (long)this.offset;
            set => this.Seek(value, SeekOrigin.Begin);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            throw new NotSupportedException(NotSupported);
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (this.ggpkStream.Position + count > (long)(this.offset + this.length))
            {
                count = (int)((long)(this.offset + this.length) - (long)(this.ggpkStream.Position + count));
            }

            if (count > 0)
            {
                return this.ggpkStream.Read(buffer, offset, count);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.ggpkStream.Seek((long)this.offset + offset, SeekOrigin.Begin);
                    return offset;

                case SeekOrigin.End:
                    this.ggpkStream.Seek((long)this.offset + (long)this.length + offset, SeekOrigin.Begin);
                    return (long)this.length + offset;

                case SeekOrigin.Current:
                    long newPosition = this.ggpkStream.Position + offset;

                    if (newPosition < (long)this.offset)
                    {
                        newPosition = (long)this.offset;
                    }

                    this.ggpkStream.Seek(newPosition, SeekOrigin.Begin);
                    return newPosition - (long)this.offset;

                default:
                    this.ggpkStream.Seek((long)this.offset, SeekOrigin.Begin);
                    return 0;
            }
        }

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException(NotSupported);
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position
        /// within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException(NotSupported);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Indicates whether managed resources shall also be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ggpkStream.Dispose();
                this.ggpkStream = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
