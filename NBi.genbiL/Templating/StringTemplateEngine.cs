using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.StringTemplate;
using NBi.Model;
using NBi.Model.Xml;

namespace NBi.GenbiL.Templating;

public class StringTemplateEngine
{

    public string TemplateXml { get; private set; }
    protected Template? Template { get; private set; }
    public string PreProcessedTemplate { get; private set; } = string.Empty;
    public string[] Variables { get; private set; }

    public StringTemplateEngine(string templateXml, string[] variables)
    {
        TemplateXml = templateXml;
        Variables = variables;
    }

    public IEnumerable<StandaloneTest> BuildTests(List<List<List<object>>> table, IDictionary<string, object> consumables)
        => Build<StandaloneTest>(table, consumables);

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
            if (obj is StandaloneTest standalone)
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
        => XmlModelSerializer.Deserialize(objectData, type);

    protected virtual string SerializeFrom(object objectData, Type type)
        => XmlModelSerializer.Serialize(objectData, type);

    public event EventHandler<ProgressEventArgs>? Progressed;
    protected virtual void InvokeProgress(ProgressEventArgs e)
    {
        Progressed?.Invoke(this, e);
    }

}
