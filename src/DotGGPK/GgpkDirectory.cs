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
using System.Collections.Generic;
#endregion

namespace DotGGPK
{
    /// <summary>
    /// Represents a directory in a ggpk archive that may contain other
    /// <see cref="GgpkDirectory">directories</see> or <see cref="GgpkFile">files</see>.
    /// </summary>
    internal class GgpkDirectory : GgpkElement, IGgpkDirectory
    {
        #region Constants and Fields

        /// <summary>
        /// Contains the subdirectories of this directory.
        /// </summary>
        private List<GgpkDirectory> directories = new List<GgpkDirectory>();

        /// <summary>
        /// Contains the files.
        /// </summary>
        private List<GgpkFile> files = new List<GgpkFile>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the parent <see cref="IGgpkDirectory">directory.</see>
        /// </summary>
        /// <value>An instance of <see cref="IGgpkDirectory"/> or <c>null</c> if this is the root element.</value>
        public IGgpkDirectory Parent { get; set; } = null;

        /// <summary>
        /// Gets the subdirectories of this directory.
        /// </summary>
        public IEnumerable<IGgpkDirectory> Directories
        {
            get
            {
                return this.directories;
            }
        }

        /// <summary>
        /// Gets the files of this directory.
        /// </summary>
        public IEnumerable<IGgpkFile> Files
        {
            get
            {
                return this.files;
            }
        }

        /// <summary>
        /// Gets the full name of the element.
        /// </summary>
        public override string FullName
        {
            get
            {
                return this.GetName(this, string.Empty);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a directory to the list of directories.
        /// </summary>
        /// <param name="directory">The directory to add.</param>
        public void Add(GgpkDirectory directory)
        {
            this.directories.Add(directory);
        }

        /// <summary>
        /// Adds a file to the list of files.
        /// </summary>
        /// <param name="file">The file to add.</param>
        public void Add(GgpkFile file)
        {
            this.files.Add(file);
        }

        #endregion
    }
}
