using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader
{
    class UrlReader : IDataSerializationReader, IDisposable
    {

        private WebClient WebClient { get; set; }
        private MemoryStream Stream { get; set; }
        private StreamReader StreamReader { get; set; }

        public IScalarResolver<string> UrlResolver { get; }

        public UrlReader(IScalarResolver<string> urlResolver)
            => UrlResolver = urlResolver;

        public TextReader Execute()
        {
            WebClient = new WebClient();
            Stream = new MemoryStream(WebClient.DownloadData(UrlResolver.Execute()));
            StreamReader = new StreamReader(Stream);
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
                WebClient.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
