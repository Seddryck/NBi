---
layout: documentation
title: Custom commands and conditions
prev_section: setup-condition
next_section: extension-installation
permalink: /docs/setup-custom/
---

It's possible to develop your own conditions or commands in C#. To achieve this, you'll need to create a C# library project and compile it as a library. At least one of the classes of your project should implement the interface *ICustomCondition* or *ICustomCommand* from the namespace *NBi.Extensibility.Decoration* and available in the nuget package *NBi.Extensibility*.

## Development of custom extensions

### Custom commands

This interface declares one method *Execute()* returning void.

The following custom condition, create a directory named *temp*:

{% highlight csharp %}
public class CustomCommandBasedOnDay : ICustomCondition
{
    public CustomConditionResult Execute()
        => {Directory.CreateDirectory("Temp\")};
}
{% endhighlight %}

### Custom conditions

This interface declares one method *Execute()* returning the result of the condition. The property *IsValid* is a boolean describing if the condition is successful (*true*) or not (*false*). The message is a *string* that will be displayed to explain why the condition is not met.

The following custom condition, returns true if the day of month is even, else returns false:

{% highlight csharp %}
public class CustomConditionBasedOnDay : ICustomCondition
{
    public CustomConditionResult Execute() 
        => new CustomConditionResult(Date.Now.Day % 2 == 0, "Oh man, retry tomorrow!");
}
{% endhighlight %}

## Referencing custom extensions

Once the custom condition is coded and built, you need to deploy the dll somewhere that your test-suite will be able to reach. In your test-suite your must reference the assembly by the mean of the xml attribute *assembly-path* and then specify the type implementing the *ICustomCondition* or *ICustomCommand* interface. Several types implementing this interface can sharethe same dll.

A set of parameters can also be defined. They will used during the construction of the object and the name of the parameters must match with the name of the constructor's attribute.

{% highlight xml %}
<condition>
  <custom assembly-path="myAssembly.dll" type="myType">
    <parameter name="firstParam">2012-10-10</parameter>
    <parameter name="secondParam">102</parameter>
  </custom>
</condition>
{% endhighlight %}

All the attributes and the parameters' value, enlisted above, accept [literal values](../primitive-scalar/#literal), and [variables](../primitive-scalar/#reference-to-a-variable) including [formatting](../primitive-scalar/#formatting) and [inline transformations](../primitive-scalar/#inline-transformations). at the exception of the parameter's name which must be a [literal value](../primitive-scalar/#literal).
