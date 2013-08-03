using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NBi.UI.Genbi.Stateful
{
    class LargeBindingList<T> : BindingList<T>
    {
        public void Clear()
        {
            this.ClearItems();
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                this.Items.Add(item);

            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded,0));
        }
    }
}
