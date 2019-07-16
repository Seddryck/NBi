using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Stateful
{
    public class CaseCollection : Dictionary<string, CaseSet>
    {
        private const string NO_NAME = "_noname";
        public string CurrentScopeName { get; set; }
        public CaseSet CurrentScope => this[CurrentScopeName];

        public CaseCollection()
        {
            this.Add(NO_NAME, new CaseSet());
            CurrentScopeName = NO_NAME;
        }

        //public CaseCollection Item(string name)
        //{
        //    if (string.IsNullOrEmpty(name))
        //        name = NO_NAME;

        //    if (!dico.Keys.Contains(name))
        //        dico.Add(name, new CaseCollection());

        //    if (dico.Count == 1)
        //        scope = name;

        //    return dico[name];
        //}

        //public bool ItemExists(string name)
        //{
        //    if (string.IsNullOrEmpty(name))
        //        name = NO_NAME;

        //    return dico.Keys.Contains(name);
        //}
    }
}
