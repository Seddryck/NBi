using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;
using NBi.Core.ResultSet.Loading;
using NBi.Core.Transformation;

namespace NBi.Core.ResultSet
{
    public class ResultSetServiceBuilder
    {
        protected bool IsSetup
        {
            get { return Loader != null; }
        }

        public IResultSetLoader Loader { private get; set; }
        private IList<TransformationProvider> Transformations { get; set; } = new List<TransformationProvider>();

        public void AddTransformation(TransformationProvider Transformation)
        {
            Transformations.Add(Transformation);
        }

        public IResultSetService GetService()
        {
            if (!IsSetup)
                throw new InvalidOperationException();

            return new ResultSetService(
                    Loader.Execute,
                    Transformations.Select<TransformationProvider, Action<ResultSet>>(x => x.Transform)
                );
        }
    }
}
