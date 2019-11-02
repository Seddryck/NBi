using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NBi.Core.Hierarchical.Json
{
    public class JsonPathFileEngine : JsonPathEngine
    {
        public string BasePath { get; }
        public IScalarResolver<string> ResolverPath { get; }
        
        public JsonPathFileEngine(IScalarResolver<string> resolverPath, string basePath, string from, IEnumerable<AbstractSelect> selects)
            : base(from, selects)
            => (BasePath, ResolverPath) = (basePath, resolverPath);

        public override IEnumerable<object> Execute()
        {
            var filePath = EnsureFileExist();

            using (var textReader = GetTextReader(filePath))
            {
                var json = JToken.ReadFrom(new JsonTextReader(textReader));
                return Execute(json);
            }
        }

        protected virtual string EnsureFileExist()
        {
            var filePath = PathExtensions.CombineOrRoot(BasePath, string.Empty, ResolverPath.Execute());
            if (!File.Exists(filePath))
                throw new ExternalDependencyNotFoundException(filePath);
            return filePath;
        }

        protected virtual TextReader GetTextReader(string filePath)
            => new StreamReader(filePath);
    }
}
