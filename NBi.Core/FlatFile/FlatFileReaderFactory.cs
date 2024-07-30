using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Configuration;
using NBi.Extensibility.FlatFile;

namespace NBi.Core.FlatFile
{
    public class FlatFileReaderFactory
    {
        protected IDictionary<string, CtorInvocation> Readers { get; private set; } = new Dictionary<string, CtorInvocation>();
        protected delegate object CtorInvocation();

        public IFlatFileReader Instantiate(string fileExtension, IFlatFileProfile profile)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                var csvProfile = new CsvProfile(profile.Attributes);
                return new CsvReader(csvProfile);
            }

            if (Readers.TryGetValue(fileExtension, out var value))
                return Instantiate(value);
            else if (Readers.TryGetValue("*.*", out var value))
                return Instantiate(value);
            throw new ArgumentException();
        }

        private IFlatFileReader Instantiate(CtorInvocation ctorInvocation) => (IFlatFileReader)ctorInvocation.Invoke();

        public FlatFileReaderFactory(IExtensionsConfiguration config)
        {
            var extensions = config?.Extensions?.Where(x => typeof(IFlatFileReader).IsAssignableFrom(x.Key) && !x.Key.IsAbstract) ?? new List<KeyValuePair<Type, IDictionary<string, string>>>();
            RegisterExtensions(extensions.ToArray());
        }

        protected internal void RegisterExtensions(IEnumerable<KeyValuePair<Type, IDictionary<string, string>>> readers)
        {
            foreach (var reader in readers)
            {
                var type = reader.Key;
                var parameters = reader.Value;

                var extension = parameters.TryGetValue("extension", out var value) ? value : "*.*";
                var ctor = type.GetConstructor([]);

                if (ctor == null)
                    throw new ArgumentException($"Can't load an extension. Can't find a constructor without parameter for the type '{type.Name}'");
                object ctorInvocation() => ctor.Invoke([]);

                if (Readers.ContainsKey(extension))
                {
                    var otherTypes = readers.Where(x => (x.Value.TryGetValue("extension", out var value) ? value : "*.*") == extension && x.Key != reader.Key).Select(x => x.Key.Name);
                    var sentence = otherTypes.Count() > 1
                        ? $"the other types '{string.Join("', '", otherTypes.Take(otherTypes.Count() - 1))}' and '{otherTypes.ElementAt(otherTypes.Count() - 1)}' are"
                        : $"another type '{otherTypes.ElementAt(0)}' is";
                    throw new ArgumentException($"Can't register an extension. The type '{type.Name}' is trying to register for the file extension '{extension}' but {sentence} also trying to register for the same file extension.");
                }

                Readers.Add(extension, ctorInvocation);
            }
        }
    }
}
