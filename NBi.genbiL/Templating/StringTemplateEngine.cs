using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Antlr4.StringTemplate;
using NBi.Xml;
using NBi.Xml.SerializationOption;

namespace NBi.GenbiL.Templating;

public class StringTemplateEngine
{
    private readonly IDictionary<Type, XmlSerializer> cacheSerializer;
    private readonly IDictionary<Type, XmlSerializer> cacheDeserializer;

    public string TemplateXml { get; private set; }
    protected Template? Template { get; private set; }
    public string PreProcessedTemplate { get; private set; } = string.Empty;
    public string[] Variables { get; private set; }

    public StringTemplateEngine(string templateXml, string[] variables)
    {
        TemplateXml = templateXml;
        Variables = variables;
        cacheSerializer = new Dictionary<Type, XmlSerializer>();
        cacheDeserializer = new Dictionary<Type, XmlSerializer>();
    }

    public IEnumerable<TestStandaloneXml> BuildTests(List<List<List<object>>> table, IDictionary<string, object> consumables)
        => Build<TestStandaloneXml>(table, consumables);

    protected internal IEnumerable<T> Build<T>(List<List<List<object>>> table, IDictionary<string, object> consumables)
    {
        InitializeTemplate(consumables);

        //For each row, we need to fill the variables and render the template. 
        int count = 0;
        foreach (var row in table)
        {
            count++;
            var str = RenderTemplate(row);

            //Cleanup the variables in the template for next iteration.
            foreach (var variable in Variables)
                Template?.Remove(variable);

            var obj = (typeof(T) == typeof(string)) ? (T)Convert.ChangeType(str, typeof(T)) : Deserialize<T>(str);
            if (obj is TestStandaloneXml standalone)
                standalone.Content = XmlSerializeFrom(standalone);
            InvokeProgress(new ProgressEventArgs(count, table.Count));
            yield return obj;
        }
    }

    protected virtual T Deserialize<T>(string value)
    {
        T obj;
        try
        { obj = XmlDeserializeFromString<T>(value); }
        catch (InvalidOperationException ex)
        { throw new TemplateExecutionException(ex.Message); }
        return obj;
    }

    protected internal virtual void InitializeTemplate(IDictionary<string, object> consumables)
    {
        var group = new TemplateGroup('$', '$');
        group.RegisterRenderer(typeof(string), new StringRenderer());
        Template = new Template(group, TemplateXml);

        //Add all the global variables (not defined in a scope)
        if (consumables != null)
            foreach (var variable in consumables)
                Template.Add(variable.Key, variable.Value);
    }

    protected internal virtual string RenderTemplate(List<List<object>> values)
    {
        for (int i = 0; i < Variables.Length; i++)
        {
            // If the variable is not initialized or if it's value is "(none)" then we skip it.
            if (!(values[i].Count == 0 || (values[i].Count == 1 && (values[i][0].ToString() == "(none)" || values[i][0].ToString() == string.Empty))))
                Template?.Add(Variables[i], values[i]);
            else
                Template?.Add(Variables[i], null);
        }

        var str = Template?.Render();
        return str ?? throw new NullReferenceException();
    }

    protected internal T XmlDeserializeFromString<T>(string objectData)
        => (T)XmlDeserializeFromString(objectData, typeof(T));

    protected internal string XmlSerializeFrom<T>(T objectData)
        => SerializeFrom(objectData!, typeof(T));


    protected virtual object XmlDeserializeFromString(string objectData, Type type)
    {
        if (!cacheDeserializer.TryGetValue(type, out var value))
        {
            var overrides = new ReadOnlyAttributes();
            overrides.Build();
            var builtDeserializer = new XmlSerializer(type, overrides);
            value = builtDeserializer;
            cacheDeserializer.Add(type, value);
        }

        var serializer = value;

        using TextReader reader = new StringReader(objectData);
        return serializer.Deserialize(reader) ?? throw new NullReferenceException();
    }

    protected virtual string SerializeFrom(object objectData, Type type)
    {
        if (!cacheSerializer.TryGetValue(type, out var value))
        {
            var overrides = new WriteOnlyAttributes();
            overrides.Build();
            var builtSerializer = new XmlSerializer(type, overrides);
            value = builtSerializer;
            cacheSerializer.Add(type, value);
        }

        var serializer = value;

        var result = string.Empty;
        using (var writer = new StringWriter())
        {
            serializer.Serialize(writer, objectData);
            result = writer.ToString();
        }
        return result;
    }

    public event EventHandler<ProgressEventArgs>? Progressed;
    protected virtual void InvokeProgress(ProgressEventArgs e)
    {
        Progressed?.Invoke(this, e);
    }

}
