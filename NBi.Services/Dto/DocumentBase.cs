using System.IO;

namespace NBi.Service.Dto
{
    /// <summary>
    /// A document
    /// </summary>
    public abstract class DocumentBase
    {
        private string fullName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentBase"/> class.
        /// </summary>
        protected DocumentBase()
        {
            this.fullName = string.Empty;
            this.Name = string.Empty;
            this.Path = string.Empty;
            this.Text = string.Empty;
            this.IsDirty = false;
        }

        /// <summary>
        /// Gets or sets the full name of the document (path + filename).
        /// </summary>
        public string FullName
        {
            get { return this.fullName; }
            set
            {
                if (this.fullName == value) return;
                var fi = new FileInfo(value);
                if (!fi.Exists) return;
                this.fullName = value;
                this.Name = fi.Name;
                this.Path = fi.Directory.FullName;
            }
        }

        /// <summary>
        /// Gets or sets the text of the document.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dirty.
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is persistent.
        /// </summary>
        public bool IsPersistent
        {
            get
            {
                return !string.IsNullOrEmpty(this.FullName);
            }
        }

        /// <summary>
        /// Gets the name of the document.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the path of the document.
        /// </summary>
        public string Path { get; private set; }
    }
}