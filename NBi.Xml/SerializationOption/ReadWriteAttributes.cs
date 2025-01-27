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

public abstract class ReadWriteAttributes : XmlAttributeOverrides
{

    public ReadWriteAttributes()
        : base()
    {
    }

    public void Build()
    {
        
#pragma warning disable 0618
        AddToArrayAttributes((TestXml t) => t.Constraints,
            new Dictionary<string, Type>()
            {
                { "subsetOf", typeof(SubsetOfOldXml) },
                { "fasterThan", typeof(FasterThanOldXml) },
                { "syntacticallyCorrect", typeof(SyntacticallyCorrectOldXml) },
                { "equalTo", typeof(EqualToOldXml) },
                { "equivalentTo", typeof(EquivalentToOldXml) },
            });

        AddToArrayAttributes((TestXml t) => t.Systems,
            new Dictionary<string, Type>()
            {
                { "resultSet", typeof(ResultSetSystemOldXml) },
            });
#pragma warning restore 0618
        AdditionalBuild();
    }

    protected abstract void AdditionalBuild();

    protected void AddAsAttribute<T, U>(Expression<Func<T, U>> expression, string alias)
    {
        var parent = GetMemberInfo(expression);
        var attrs = new XmlAttributes() { XmlAttribute = (new XmlAttributeAttribute(alias)) };
        Add(parent.DeclaringType ?? throw new NullReferenceException(), parent.Name, attrs);
    }

    protected void AddAsElement<T, U>(Expression<Func<T, U>> expression, string alias)
    {
        var parent = GetMemberInfo(expression);
        var attrs = new XmlAttributes();
        attrs.XmlElements.Add(new XmlElementAttribute(alias));
        Add(parent.DeclaringType ?? throw new NullReferenceException(), parent.Name, attrs);
    }

    protected void AddAsElement<T, U>(Expression<Func<T, U>> expression, string alias, int order)
    {
        var parent = GetMemberInfo(expression);
        var attrs = new XmlAttributes();
        var attr = new XmlElementAttribute(alias) { Order = order };
        attrs.XmlElements.Add(attr);
        Add(parent.DeclaringType ?? throw new NullReferenceException(), parent.Name, attrs);
    }

    protected void AddAsIgnore<T, U>(Expression<Func<T, U>> expression, bool value = true)
    {
        var parent = GetMemberInfo(expression);
        var attrs = new XmlAttributes() { XmlIgnore = value };
        Add(parent.DeclaringType ?? throw new NullReferenceException(), parent.Name, attrs);
    }

    protected void AddAsText<T, U>(Expression<Func<T, U>> expression)
    {
        var parent = GetMemberInfo(expression);
        var attrs = new XmlAttributes() { XmlText = new XmlTextAttribute() };
        Add(parent.DeclaringType ?? throw new NullReferenceException(), parent.Name, attrs);
    }

    protected void AddAsAnyNotIgnore<T, U>(Expression<Func<T, U>> expression)
    {
        var parent = GetMemberInfo(expression);
        var attrs = new XmlAttributes() { XmlIgnore = false };
        attrs.XmlAnyElements.Add(new XmlAnyElementAttribute());
        Add(parent.DeclaringType ?? throw new NullReferenceException(), parent.Name, attrs);
    }

    protected void AddToArrayAttributes<T, U>(Expression<Func<T, U>> expression, Dictionary<string, Type> mappings)
    {
        var parent = GetMemberInfo(expression);
        var arrayAttr = (XmlArrayAttribute)parent.GetCustomAttributes(typeof(XmlArrayAttribute), false)[0];
        var arrayItemAttrs = parent.GetCustomAttributes(typeof(XmlArrayItemAttribute), false).Cast<XmlArrayItemAttribute>().ToList();
        var attrs = new XmlAttributes() { XmlArray = arrayAttr };
        arrayItemAttrs.ForEach(i => attrs.XmlArrayItems.Add(i));
        foreach (var key in mappings.Keys)
            attrs.XmlArrayItems.Add(new XmlArrayItemAttribute(key, mappings[key]));
        Add(parent.DeclaringType ?? throw new NullReferenceException(), parent.Name, attrs);
    }

    protected void AddToElements<T, U>(Expression<Func<T, U>> expression, string alias, Type aliasType)
    {
        var parent = GetMemberInfo(expression);
        var arrayAttr = parent.GetCustomAttributes(typeof(XmlElementAttribute), false).Cast<XmlElementAttribute>().ToList();
        var attrs = new XmlAttributes();
        arrayAttr.ForEach(i => attrs.XmlElements.Add(i));
        attrs.XmlElements.Add((new XmlElementAttribute(alias, aliasType)));
        Add(parent.DeclaringType ?? throw new NullReferenceException(), parent.Name, attrs);
    }


    /// <summary>
    /// Extracts the PropertyInfo for the property being accessed in the given expression.
    /// </summary>
    /// <remarks>
    /// If possible, the actual owning type of the property is used, rather than the declaring class (so if "x" in "() => x.Foo" is a subclass overriding "Foo", then x's PropertyInfo for "Foo" is returned rather than the declaring base class's PropertyInfo for "Foo").
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="propertyExpression"></param>
    /// <returns></returns>
    private MemberInfo GetMemberInfo<T, U>(Expression<Func<T, U>> propertyExpression)
    {
        var memberExpression = propertyExpression?.Body as MemberExpression
            ?? throw new ArgumentException($"Expression not a MemberExpresssion: {propertyExpression}", nameof(propertyExpression));

        var realType = memberExpression.Expression?.Type
            ?? throw new ArgumentException($"Member expression '{memberExpression}' has no DeclaringType: {propertyExpression})");

        return realType.GetProperty(memberExpression.Member.Name)
            ?? throw new ArgumentException($"Type '{realType}' has no property '{memberExpression.Member.Name}')");
    }

    private string GetXmlName(string input) => string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString().ToLowerInvariant()));
}