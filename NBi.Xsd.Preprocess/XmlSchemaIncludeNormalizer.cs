using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace NBi.Xsd.Preprocess
{
    // A class to remove all <include> from a Xml Schema
    //
    public class XmlSchemaIncludeNormalizer
    {
        // Takes as input a XmlSchema which has includes in it 
        // and the schema location uri of that XmlSchema
        // 
        // Returns a "preprocessed" form of XmlSchema without any 
        // includes. It still retains imports though. Also, it does
        // not propagate unhandled attributes
        //
        // It can throw any exception
        public static XmlSchema BuildIncludeFreeXmlSchema(XmlSchema inSch)
        {
            XmlSchema outSch = new XmlSchema();

            AddSchema(outSch, inSch);

            return outSch;
        }

        // Adds everything in the second schema minus includes to 
        // the first schema
        //
        private static void AddSchema(XmlSchema outSch, XmlSchema add)
        {
            outSch.AttributeFormDefault = add.AttributeFormDefault;
            outSch.BlockDefault = add.BlockDefault;
            outSch.ElementFormDefault = add.ElementFormDefault;
            outSch.FinalDefault = add.FinalDefault;
            outSch.Id = add.Id;
            outSch.TargetNamespace = add.TargetNamespace;
            outSch.Version = add.Version;

            AddTableToSchema(outSch, add.AttributeGroups);
            AddTableToSchema(outSch, add.Attributes);
            AddTableToSchema(outSch, add.Elements);
            AddTableToSchema(outSch, add.Groups);
            AddTableToSchema(outSch, add.Notations);
            AddTableToSchema(outSch, add.SchemaTypes);

            // Handle includes as a special case
            for (int i = 0; i < add.Includes.Count; i++)
            {
                if (!(add.Includes[i] is XmlSchemaInclude))
                    outSch.Includes.Add(add.Includes[i]);
            }
        }

        // Adds all items in the XmlSchemaObjectTable to the specified XmlSchema
        //
        private static void AddTableToSchema(XmlSchema outSch, XmlSchemaObjectTable table)
        {
            var e = table.GetEnumerator();

            while (e.MoveNext())
            {
                outSch.Items.Add((XmlSchemaObject)(e.Value ?? throw new NullReferenceException()));
            }
        }
    }
}
