using NBi.Core.Transformation.Transformer.Native;
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
        public INativeTransformation Instantiate(string code)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;

            var parameters = code.Replace("(", ",")
                .Replace(")", ",").Trim()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList().Skip(1).Select(x => x.Trim()).ToArray();

            var classToken = code.Contains("(") ? code.Substring(0, code.IndexOf('(')).Replace(" ", "") : code;
            var className = textInfo.ToTitleCase(classToken.Trim().Replace("-", " ")).Replace(" ", "").Replace("Datetime", "DateTime");

            var clazz = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes())
                       .Where(
                                t => t.IsClass
                                && t.IsAbstract == false
                                && t.Name == className
                                && t.GetInterface("INativeTransformation") != null)
                       .SingleOrDefault();

            if (clazz == null)
                throw new NotImplementedTransformationException(className);

            return (INativeTransformation)Activator.CreateInstance(clazz, parameters);
        }
    }
}
