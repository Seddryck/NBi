using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using NBi.Xml.Constraints;
using NBi.Xml.Constraints.Comparer;
using NBi.Xml.Items.Calculation;

namespace NBi.Xml.SerializationOption
{
    public class ReadOnlyAttributes : XmlAttributeOverrides
    {

        public ReadOnlyAttributes()
            : base()
        {
        }

        public void Build()
        {
            #pragma warning disable 0618
            AddAsAttribute((TestXml t) => t.Description, "description");
            AddAsAttribute((TestXml t) => t.Ignore, "ignore");
            AddAsAttribute((ContainXml c) => c.Caption, "caption");

            AddAsElement((NoRowsXml c) => c.InternalAliasesOld, "variable");
            AddAsElement((FilterXml f) => f.InternalAliasesOld, "variable");

            AddAsAttribute((PredicationXml p) => p.Name, "name");

            AddToArrayyAttributes((TestXml t) => t.Constraints, "subsetOf", typeof(SubsetOf1xXml));
            AddToElements((PredicationXml p) => p.Predicate, "within-list", typeof(WithinListXml));
            #pragma warning restore 0618
        }

        private void AddAsAttribute<T, U>(Expression<Func<T, U>> expression, string alias)
        {
            var parent = GetMemberInfo(expression);
            var attrs = new XmlAttributes() { XmlAttribute = (new XmlAttributeAttribute(alias)) };
            Add(parent.DeclaringType, parent.Name, attrs);
        }

        private void AddAsElement<T, U>(Expression<Func<T, U>> expression, string alias)
        {
            var parent = GetMemberInfo(expression);
            var attrs = new XmlAttributes();
            attrs.XmlElements.Add(new XmlElementAttribute(alias));
            Add(parent.DeclaringType, parent.Name, attrs);
        }

        private void AddToArrayyAttributes<T, U>(Expression<Func<T, U>> expression, string alias, Type aliasType)
        {
            var parent = GetMemberInfo(expression);
            var arrayAttr = (XmlArrayAttribute)parent.GetCustomAttributes(typeof(XmlArrayAttribute), false)[0];
            var arrayItemAttrs = parent.GetCustomAttributes(typeof(XmlArrayItemAttribute), false).Cast<XmlArrayItemAttribute>().ToList();
            var attrs = new XmlAttributes() { XmlArray = arrayAttr };
            arrayItemAttrs.ForEach(i => attrs.XmlArrayItems.Add(i));
            attrs.XmlArrayItems.Add(new XmlArrayItemAttribute(alias, aliasType));
            Add(parent.DeclaringType, parent.Name, attrs);
        }

        private void AddToElements<T, U>(Expression<Func<T, U>> expression, string alias, Type aliasType)
        {
            var parent = GetMemberInfo(expression);
            var arrayAttr = parent.GetCustomAttributes(typeof(XmlElementAttribute), false).Cast<XmlElementAttribute>().ToList();
            var attrs = new XmlAttributes();
            arrayAttr.ForEach(i => attrs.XmlElements.Add(i));
            attrs.XmlElements.Add((new XmlElementAttribute(alias, aliasType)));
            Add(parent.DeclaringType, parent.Name, attrs);
        }

        private MemberInfo GetMemberInfo<T, U>(Expression<Func<T, U>> expression)
        {
            if (expression.Body is MemberExpression member)
                return member.Member;

            throw new ArgumentException("Expression is not a member access", "expression");
        }

        private string GetXmlName(string input) => string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString().ToLowerInvariant()));
    }
}