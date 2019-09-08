using NBi.Core.Transformation.Transformer.Native;
using NBi.Core.Transformation.Transformer.Native.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer
{
    public class NativeTransformationFactory
    {
        protected string BasePath { get; }
        public NativeTransformationFactory() : this(string.Empty) { }
        public NativeTransformationFactory(string basePath) => BasePath = basePath;
        public INativeTransformation Instantiate(string code)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;

            var parameters = code.Replace("(", ",")
                .Replace(")", ",").Trim()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList().Skip(1).Select(x => x.Trim()).ToList();

            var classToken = code.Contains("(") ? code.Substring(0, code.IndexOf('(')).Replace(" ", "") : code;
            var className = textInfo.ToTitleCase(classToken.Trim().Replace("-", " ")).Replace(" ", "").Replace("Datetime", "DateTime").Replace("Timespan", "TimeSpan");

            var type = typeof(INativeTransformation).Assembly.GetTypes()
                       .Where(
                                t => t.IsClass
                                && t.IsAbstract == false
                                && t.Name == className
                                && t.GetInterface(typeof(INativeTransformation).Name) != null)
                       .SingleOrDefault();

            if (type == null)
                throw new NotImplementedTransformationException(className);

            if (typeof(IBasePathTransformation).IsAssignableFrom(type))
                parameters.Insert(0, BasePath);

            return (INativeTransformation)Activator.CreateInstance(type, parameters.ToArray());
        }
    }
}
