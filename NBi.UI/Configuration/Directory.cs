using System;
using System.Collections.Generic;
using System.Text;

namespace NBi.UI.Configuration
{
    public class Directory
    {
        public string Path { get; set; }
        protected DirectoryCollection Root { get; private set; }
        public string File { get; set; }

        public string FullPath
        {
            get
            {
                var path = Root.Root;

                if (!Root.Root.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) && !String.IsNullOrEmpty(Root.Root))
                    path += System.IO.Path.DirectorySeparatorChar;

                path += Path;

                if (!Path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) && !String.IsNullOrEmpty(Path))
                    path += System.IO.Path.DirectorySeparatorChar;

                return path;
            }
        }

        public string FullFileName
        {
            get 
            {
                var fullPath = Root.Root;

                if (!Root.Root.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) && !String.IsNullOrEmpty(Root.Root))
                    fullPath += System.IO.Path.DirectorySeparatorChar;

                fullPath += Path;

                if (!Path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) && !String.IsNullOrEmpty(Path))
                    fullPath += System.IO.Path.DirectorySeparatorChar;

                fullPath += File;

                return fullPath;
            }
        }

        public Directory(DirectoryCollection root)
        {
            Root = root;
        }
    }
}
