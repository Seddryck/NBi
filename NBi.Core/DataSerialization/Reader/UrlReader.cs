using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader;

class UrlReader : IDataSerializationReader, IDisposable
{

    private HttpClient Client { get; }
    private MemoryStream? Stream { get; set; }
    private StreamReader? StreamReader { get; set; }

    public IScalarResolver<string> UrlResolver { get; }

    public UrlReader(HttpClient client, IScalarResolver<string> urlResolver)
        => (Client, UrlResolver) = (client, urlResolver);

    public UrlReader(IScalarResolver<string> urlResolver)
        : this(new HttpClient(), urlResolver) { }

    public TextReader Execute()
    {
        var response = Client.GetAsync(UrlResolver.Execute() ?? throw new NullReferenceException()).Result;
        response.EnsureSuccessStatusCode();

        var responseStream = response.Content.ReadAsStreamAsync().Result;
        StreamReader = new StreamReader(responseStream);
        return StreamReader;
    }

    bool disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            StreamReader?.Dispose();
            Stream?.Dispose();
            Client?.Dispose();
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
