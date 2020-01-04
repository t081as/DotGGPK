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
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents the abstract base class for <see cref="GgpkDirectory">directories</see>
    /// and <see cref="GgpkFile">files</see>.
    /// </summary>
    internal abstract class GgpkElement : IGgpkElement
    {
        #region Properties

        /// <summary>
        /// Gets or sets the file name of the ggpk archive file.
        /// </summary>
        public string ArchiveFileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the timestamp of the element.
        /// </summary>
        public uint TimeStamp { get; set; } = 0;

        /// <summary>
        /// Gets or sets the SHA-256 hash of the data.
        /// </summary>
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Gets the full name of the element.
        /// </summary>
        public abstract string FullName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the full name of the element (including the names of the parents).
        /// </summary>
        /// <param name="currentDirectory">The current <see cref="IGgpkDirectory"/>.</param>
        /// <param name="name">The name.</param>
        /// <returns>The new name including the name of the current <see cref="IGgpkDirectory"/>.</returns>
        protected virtual string GetName(IGgpkDirectory currentDirectory, string name)
        {
            IGgpkDirectory element = currentDirectory;
            string result = name;

            while (element != null)
            {
                if (!string.IsNullOrEmpty(element.Name))
                {
                    result = $"{element.Name}/{result}";
                }

                element = element.Parent;
            }

            return $"/{result}";
        }

        #endregion
    }
}
