using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using NBi.Xml.Constraints;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Decoration.Command;
using NBi.Xml.Items;
using NBi.Xml.Items.Alteration.Transform;
using NBi.Xml.Items.Calculation;
using NBi.Xml.Items.ResultSet;
using NBi.Xml.Settings;
using NBi.Xml.Systems;

namespace NBi.Xml.SerializationOption;

public class ReadOnlyAttributes : ReadWriteAttributes
{

    public ReadOnlyAttributes()
        : base() { }

    protected override void AdditionalBuild()
    {
#pragma warning disable 0618
        AddAsAttribute((TestXml t) => t.Description, "description");
        AddAsAttribute((TestXml t) => t.Ignore, "ignore");
        AddAsAttribute((ContainXml c) => c.Caption, "caption");
        AddAsAttribute((TransformXml t) => t.ColumnOrdinal, "column-index");
        AddAsAttribute((BaseItem x) => x.ConnectionStringOld, "connectionString");
        AddAsAttribute((ConnectionWaitXml c) => c.SpecificConnectionStringOld, "connectionString");
        AddAsAttribute((DataManipulationAbstractXml x) => x.SpecificConnectionStringOld, "connectionString");
        AddAsAttribute((SqlRunXml x) => x.SpecificConnectionStringOld, "connectionString");
        AddAsAttribute((SinglePredicationXml p) => p.Name, "name");
        AddAsAttribute((ResultSetSystemXml r) => r.FilePath, "file");

        AddAsElement((NoRowsXml c) => c.InternalAliasesOld, "variable", 2);
        AddAsElement((FilterXml f) => f.InternalAliasesOld, "variable");
        AddAsElement((ColumnDefinitionXml c) => c.InternalTransformationInner, "transformation");
        AddAsElement((DefaultXml x) => x.ConnectionStringOld, "connectionString");
        AddAsElement((ReferenceXml x) => x.ConnectionStringOld, "connectionString");

        AddAsText((FileXml x) => x.Value);

        AddToElements((SinglePredicationXml p) => p.Predicate, "within-list", typeof(WithinListXml));

        AddToElements((ProjectionOldXml x) => x.ResultSetOld, "resultSet", typeof(ResultSetSystemXml));
        AddToElements((LookupExistsXml x) => x.ResultSetOld, "resultSet", typeof(ResultSetSystemXml));
        AddToElements((LookupMatchesXml x) => x.ResultSetOld, "resultSet", typeof(ResultSetSystemXml));
#pragma warning restore 0618
    }
}