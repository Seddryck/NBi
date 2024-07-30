using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataSerialization.Reader
{
    class ScalarReader : IDataSerializationReader, IDisposable
    {
        private IScalarResolver<string> ScalarResolver { get; }
        private MemoryStream? Stream { get; set; }
        private StreamReader? StreamReader { get; set; }

        public ScalarReader(IScalarResolver<string> scalarResolver)
            => ScalarResolver = scalarResolver;

        public TextReader Execute()
        {
            Stream = new MemoryStream(Encoding.UTF8.GetBytes(ScalarResolver.Execute() ?? string.Empty));
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
