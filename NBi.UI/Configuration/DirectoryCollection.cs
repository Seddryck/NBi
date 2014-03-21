using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Configuration
{
    public class DirectoryCollection : IEnumerable<Directory>
    {
        public IEnumerator GetEnumerator()
        {
            return _dico.GetEnumerator();
        }

        IEnumerator<Directory> IEnumerable<Directory>.GetEnumerator()
        {
            foreach (var item in _dico)
            {
                yield return item.Value;
            }
        }


        public enum DirectoryType
        {
            Metadata,
            Query,
            Expect,
            Actual,
            TestSuite
        }
        
        public string Root { get; set; }
        protected Dictionary<DirectoryType, Directory> _dico;

        public DirectoryCollection()
        {
            _dico = new Dictionary<DirectoryType, Directory>();
        }

        public Directory this[DirectoryType i]
        {
            get
            {
                if (!_dico.ContainsKey(i))
                    _dico[i] = new Directory(this); 
                return _dico[i];
                
            }
            set
            {
                _dico[i] = value;
            }
        } 
    }
}
