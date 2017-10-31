using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;
using NBi.Core.ResultSet.Loading;
using NBi.Core.Transformation;
using NBi.Core.ResultSet.Alteration;

namespace NBi.Core.ResultSet
{
    public class ResultSetServiceBuilder
    {
        protected bool IsSetup
        {
            get { return load != null; }
        }

        private Load load;
        private List<Alter> alters = new List<Alter>();

        public IResultSetService GetService()
        {
            if (!IsSetup)
                throw new InvalidOperationException();

            return new ResultSetService(load, alters);
        }

        public void Setup(IResultSetLoader loader)
        {
            if (load != null)
                throw new InvalidOperationException("You can't define more than one load method.");
            load = loader.Execute;
        }

        public void Setup(Load load)
        {
            if (this.load != null)
                throw new InvalidOperationException("You can't define more than one load method.");
            
            this.load = load ?? throw new ArgumentNullException(nameof(load));
        }

        public void Setup(IEnumerable<Alter> alterations)
        {
            foreach (var alteration in alterations)
                Setup(alteration);
        }

        public void Setup(Alter alteration)
        {
            alters.Add(alteration);
        }
        
    }
}
