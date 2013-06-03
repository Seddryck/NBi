using System;

namespace NBi.UI.Configuration
{
    public class Directory
    {
        public string Path { get; set; }
        protected DirectoryCollection Root { get; private set; }
        public string File { get; set; }

        /// <summary>
        /// Returns the full path excluding the filename and extension
        /// </summary>
        public string FilenameWithoutExtension
        {
            get
            {
                if (!string.IsNullOrEmpty(File))
                    return System.IO.Path.GetFileNameWithoutExtension(File);
                else
                    return null;
            }
        }

        /// <summary>
        /// Returns the full path excluding the filename and extension
        /// </summary>
        public string FullPath
        {
            get
            {
                var path = Root.Root;

                if (string.IsNullOrEmpty(Root.Root) && string.IsNullOrEmpty(Path))
                    return string.Empty;

                if (!String.IsNullOrEmpty(Root.Root) && !Root.Root.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                    path += System.IO.Path.DirectorySeparatorChar;

                path += Path;

                if (!String.IsNullOrEmpty(Path) && !Path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                    path += System.IO.Path.DirectorySeparatorChar;

                return path;
            }
        }

        /// <summary>
        /// Returns the full path including the filename and extension if they exist.
        /// </summary>
        public string FullFileName
        {
            get 
            {
                var fullPath = FullPath;

                fullPath += File;

                return fullPath;
            }
            set
            {
                if (string.IsNullOrEmpty(Root.Root) || value.StartsWith(Root.Root))
                {
                    //Define File value
                    File = System.IO.Path.GetFileName(value);
                    
                    //Define Path Value
                    if (string.IsNullOrEmpty(Root.Root))
                        Path = System.IO.Path.GetDirectoryName(value);
                    else if (System.IO.Path.GetDirectoryName(value).Length > Root.Root.Length)
                        Path = System.IO.Path.GetDirectoryName(value).Substring(Root.Root.Length);
                    else
                        Path = "";
                }
            }
        }

        public Directory(DirectoryCollection root)
        {
            Root = root;
        }
    }
}
