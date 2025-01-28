using NBi.Core.Api.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader;

class RestReader : IDataSerializationReader, IDisposable
{
    private TextReader? TextReader { get; set; }
    public RestEngine Rest { get; }

    public RestReader(RestEngine rest)
        => (Rest) = (rest);

    public TextReader Execute()
    {
        TextReader = new StringReader(Rest.Execute());
        return TextReader;
    }

    #region "Disposable"

    bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            TextReader?.Dispose();
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
