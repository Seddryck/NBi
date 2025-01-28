using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resource;

class Klass
{
    public Klass()
    {

    }

    public string ExecutePublicString(string paramString)
    {
        if (paramString == "MyString")
            return "Executed";
        return "Incorrect Parameters";
    }

    private string ExecutePrivateString(string paramString)
    {
        if (paramString == "MyString")
            return "Executed";
        return "Incorrect Parameters";
    }

    public string ExecuteDoubleParam(DateTime paramDateTime, Enumeration paramEnum)
    {
        if (paramDateTime == new DateTime(2012,05,07) && paramEnum == Enumeration.Beta)
            return "Executed";
        return "Incorrect Parameters";
    }
}
