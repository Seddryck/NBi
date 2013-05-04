using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface
{
    public interface IView
    {
        event EventHandler Initialize;
        //event EventHandler Load;

        void ShowException(string text);
        void ShowInform(string text);
    }
}
