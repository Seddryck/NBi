---
layout: documentation
title: Result-set
prev_section: primitive-sequence
next_section: scalar-transform
permalink: /docs/primitive-result-set/
---
Two-dimensional size-mutable, potentially heterogeneous tabular data structure with labeled axes (rows and columns). The primary NBi data structure.

## Definition

It's possible to specify a result-set in different ways.

### Inline definition

The most straightforward is to define rows and cells inline. This is relatively useful when your result-set is small.

{% highlight xml %}
<result-set>
  <row>
    <cell>Canada</cell>
    <cell>130</cell>
  </row>
  <row>
    <cell>Belgium</cell>
    <cell>45</cell>
  </row>
</result-set>
{% endhighlight %}

#### Null and empty values

To define a cell with a value equal to *null*, you'll have to use the notation with brackets or the auto-closing xml element.

The following row contains two cells with a *null* value:
{% highlight xml %}
<row>
  <cell>(null)</cell>
  <cell/>
</row>
{% endhighlight %}

To define a cell with a value equal to *empty*, you'll have to use the notation with brackets or an empty xml element.

The following row contains two cells with an *empty* value:
{% highlight xml %}
<row>
  <cell>(empty)</cell>
  <cell></cell>
</row>
{% endhighlight %}

### External definition

You can also refer to an external flat file. By default, flat files are considered as CSV with a field-separator set to a semi-column (*;*) and a record-separator set to carriage return/line feed (*CrLf*) and no quoting character. You can edit this default format as explained in [this section](../config-profile-csv/).

{% highlight xml %}
<result-set file="myFile.csv"/>
{% endhighlight %}

the filename can be dynamically evaluated based on a variable (formatting). To enable this feature, you must precede the filename by a tilt ```~``` and mix static part of the filename with dynamic part. The dynamic part must be contained between curly braces ```{}``` and is starting by the variable's name to consider.

{% highlight xml %}
<result-set file="File_{@myVar}.csv"/>
{% endhighlight %}

Using the previous notation, if the value of *myVar* is *2018* then the filename *File_2018.csv* will be considered for loading the result-set.

In case the variable is a numeric or dateTime, it can be useful to format it. This formatting must be specified after a column (```:```).

{% highlight xml %}
<result-set file="File_{@myDate:yyyy}_{@myDate:MM}.csv"/>
{% endhighlight %}

Using the previous notation, if the value of *myVar* is *1st January 2018* then the filename *File_2018_01.csv* will be considered for loading the result-set.

The formatting syntax is the one supported by .Net and explained in MSDN for the [numerics](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings) and [dateTimes](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings)

You can also use the long version to reference an external flat file:
{% highlight xml %}
<result-set>
  <file>
    <path>File_{@myDate:yyyy}_{@myDate:MM}.csv</path>
  </file>
</result-set>
{% endhighlight %}

#### if-missing directive

If the mentioned file is not available, by default, the test will throw an error stating that a dependency has not been found. It's possible to override this behaviour and specify that you should take a look to another location in case of a missing file. This is a recursive statement, so it's possible to define a third or fourth location to use when the two first are not available and so on.

{% highlight xml %}
<result-set>
  <file>
    <path>File_{@myDate:yyyy}_{@myDate:MM}.csv</path>
    <if-missing>
      <file>
        <path>AnotherFile_{@myDate:yyyy}_{@myDate:MM}.csv</path>
      </file>
    </if-missing>
  </file>
</result-set>
{% endhighlight %}

#### Custom parser

If you need, you can also define a custom parser. More information are available at [this page](../extension-flatfile/).

{% highlight xml %}
<result-set>
  <file>
    <path>File_{@myDate:yyyy}_{@myDate:MM}.csv</path>
    <parser name="opendata"/>
  </file>
</result-set>
{% endhighlight %}

### Sequences-based definition

You can define a result-set as the combination of one or more sequences. Each sequence creates a new column in the result-set. The resulting rows' count is depending on the combination type. Currently, the only combination-type supported is a *cartesian-product*. The *cartesian-product* will create one row for each combination of the different elements of the two sequences.

In the following definition, the two sequences contain 2 elements and 3 elements. The result of this combination will be a result-set with 2 columns (first of type *text*, next of type *dateTime*) and 6 rows.

{% highlight xml %}
<result-set>
  <sequences-combination operation="cartesian-product">
    <sequence type="text">
      <item>be</item>
      <item>fr</item>
    </sequence>
    <sequence type="dateTime">
      <loop-sentinel seed="2015-01-01" terminal="2017-01-01" step="1 year"/>
    </sequence>
  </sequences-combination>
</result-set>
{% endhighlight %}

### Single sequence definition

You can define a result-set as a single sequence. Naturally, the resulting result-set will have one column and the count of rows will be equal to the count of items in the sequence.

{% highlight xml %}
<result-set>
  <sequence type="text">
    <item>be</item>
    <item>fr</item>
  </sequence>
</result-set>
{% endhighlight %}

In the example above, the result-set will have a unique column with two rows containing the values *be* and *fr*.

### Query-based definition

Naturally, all the queries defined here under can take advantage of all features: [parameters](../query-parameter), [template-variables](../query-template), [timeout](../query-timeout).

#### Inline query

This query can be sourced from an inline definition

{% highlight xml %}
<result-set>
  <query>
    select * from myTable
  </query>
<result-set>
{% endhighlight %}

#### Query defined in an external file

{% highlight xml %}
<result-set>
  <query file="myQuery.sql"/>
<result-set>
{% endhighlight %}

#### Query defined in an assembly's method

More info about [assembly](../docs/query-assembly)

{% highlight xml %}
<result-set>
  <query>
    <assembly ...>
  <query>
<result-set>
{% endhighlight %}

#### Query defined in a report (SQL Server Reporting Server)

More info about [report](../docs/query-report#dataset)

{% highlight xml %}
<result-set>
  <query>
    <report ...>
  <query>
<result-set>
{% endhighlight %}

#### Query defined in a shared dataset (SQL Server Reporting Server)

More info about [shared-dataset](../docs/shared-dataset)

{% highlight xml %}
<result-set>
  <query>
    <shared-dataset ...>
  </query>
</result-set>
{% endhighlight %}

### Empty

It's possible to define an empty *result-set* meaning a *result-set* with zero row. To define how many column will be available in the *result-set*, you must specify the *column-count* attribute.

{% highlight xml %}
<result-set>
  <empty column-count="2"/>
</result-set>
{% endhighlight %}

It's also possible to define the name of the columns. To do this, just add a child element *column* with an attribute *identifier* set to the name of the column.

{% highlight xml %}
<result-set>
  <empty>
    <column identifier="foo"/>
    <column identifier="bar"/>
  </empty>
</result-set>
{% endhighlight %}

You can combine both approaches and specify the column count and a few column names. If you've more named columns than *column-count* then the *column-count* value is ignored. The column names are applied to the first columns of your result-set/

### Xml source

It's possible to define a result-set based on an xml document and an XPath expression. The latest will transform the hierarchical view (xml) into a flat view (result-set).

The xml document can be a file, a url or the result of a call to a REST API.

To achieve this, you'll need to define the *file* or *url* information to locate the xml content. For a call to the REST API go to the paragraph for [REST API](#Rest-API).

{% highlight xml %}
<result-set>
  <xml-source>
    <file>..\xml\myFile.xml</file>
    ...
  <xml-source>
<result-set>
{% endhighlight %}

or 

{% highlight xml %}
<result-set>
  <xml-source>
    <url>http://www.xml.com/myXml</url>
    ...
  <xml-source>
<result-set>
{% endhighlight %}

The second part of the definition of the *xml-source* is to specify the XPath expression. This expression supports XPath 1.0. You'll have to define the level of expected information by the means of the *from* element. Based on this level, you can define the elements to select. You can achieve this by specifying one or more *select* elements and the relative path (relative to the *from* element) to the element. If you want to select an attribute, specify the name of the expected attribute in the *attribute* attribute.

{% highlight xml %}
<result-set>
  <xml-source>
    ...
    <xpath>
      <select>.</select>
      <select attribute="myAttribute">.</select>
    </xpath>
  <xml-source>
<result-set>
{% endhighlight %}

#### Namespaces management

XPath 1.0 has a huge limitation and is not able to deal with default namespace without prefix. To work-around this limitation NBi supports two tricks.

If the default namespace is the only namespace or if the intersection of two namespaces is empty, you can ask NBi to not take into account the namespaces at all by specifying the attribute *ignore-namespaces* at the level of the *xml-source*.

For other cases, it's recommended to specify a prefix for the default namespace and use it in your XPath expression. To specify the prefix for the default namespace, you'll use the *default-namespace-attribute* at the level of the *xpath* element.

### JSON source

Mst of the features described for an Xml are also available for a Json. You'll also have to supply a FLOWR query on the form of Json-paths. As JSON doesn't support namespace and attributes, the related features are not supported.

{% highlight xml %}
<result-set>
  <json-source>
    <file>...</file>
    <json-path>
      <select>$</select>
      <select>$elements[*].foo</select>
    </json-path>
  <json-source>
<result-set>
{% endhighlight %}

Note that the default implementation of JSON path doesn't support to go back to a parent level. This version of Json Path is supporting ```!``` has a way to indicate that you'll like to go to the ancestror. This syntax is only supported at the beginning of the JSON path.

### Rest API

For Json and Xml sources, it's possible to stipulate that the content to query with the XPath or JsonPath is the result of the call to a Rest-API. At this moment, it only supports to analyze results with an HTTP code equal to 200 (success) and only submit GET requests.

The element *rest* is expected in place of *url* or *file* as children of *xml-source* or *json-source*.

{% highlight xml %}
<result-set>
  <json-source>
    <rest>
     ...
    </rest>
  <json-source>
<result-set>
{% endhighlight %}

The attribute *base-url* specififes the base url of the Rest API. It supports HTTP and HTTPS.

#### Authentication

The element *authentication* specifies how NBi will authenticate when calling the Rest API. If not specified, NBi will assume that you're trying to use *anonymous* authentication. You can also specifies this choice by using the element *anonymous* within the *authentication* node.

{% highlight xml %}
<rest>
  <authentication>
    <anonymous/>
  <authentication>
</rest>
{% endhighlight %}

Another option is to rely on *http-basic* authentication. On this element, you should define the *username* and *password* attributes, corresponding to the related informations.

{% highlight xml %}
<rest>
  <authentication>
    <http-basic username="foo" password="bar"/>
  <authentication>
</rest>
{% endhighlight %}

It also supports NTML authentication. To use this protocal, you can specify that you want to use the current user or another user. For the current user, just specify the element *ntml-current-user*.

{% highlight xml %}
<rest>
  <authentication>
    <ntml-current-user/>
  <authentication>
</rest>
{% endhighlight %}

If you want to change the user, you'll have to specify the *username* and *password* attributes, corresponding to the related informations.

{% highlight xml %}
<rest>
  <authentication>
    <ntml username="domain\foo" password="bar"/>
  <authentication>
</rest>
{% endhighlight %}

Another option is the usage of an API key. To select this kind of authentication add a child element *api-key* to the element *authentication*. The minimal information that you should provide is the value of the API in the node *api-key*. If the name of the API key is not ```apiKey```, you can override it on the attribute *name*.

{% highlight xml %}
<rest>
  <authentication>
    <api-key name="foo">bar</api-key>
  <authentication>
</rest>
{% endhighlight %}

The last option is to use OAuth2. NBi doesn't implement an end-to-end implementation of OAuth2 because there are so many ways to implement OAuth2 that it would be impossible to support all of them. NBi is expecting that you're retrieving the token by the mean of a [varriable](../variable-defined/) and that you specify it in the child element *access-token*. You also need to specify the *token-type* based on the kind of token used by your OAuth2 authentication method.

{% highlight xml %}
<rest>
  <authentication>
    <oauth2 token-type="bearer">
      <access-token>@oauth2Token<access-token>
    </oauth2>
  <authentication>
</rest>
{% endhighlight %}

#### Rest Query

The rest query can partially or fully specified at many places: headers, paths, parameters and segments.

The first portential child element that you can specifiy is the *header* element. You can specify multiple headers if needed. For each of them, you'll provide the name of the header and it's value.

{% highlight xml %}
<rest>
  <header name="foo">bar</header>
</rest>
{% endhighlight %}

The second portential child element that you can specifiy is the *path* element. This path will be added to the *base-url* specifies above. In the example bellow, the url *https://www.myapi.com/v2/users* is defined in two parts: the *base-url* and *the path*.

{% highlight xml %}
<rest base-url="https://www.myapi.com/v2/">
  <path>users</path>
</rest>
{% endhighlight %}

The path can also contains some segments that must be replaced by a value. To specify a part of the path that must be replaced by a value, you must enclose it between curly braces ```{}```. This part will be replaced by the value specified in the child element *segment* with the corresponding name.

{% highlight xml %}
<rest base-url="https://www.myapi.com/v2/">
  <path>users/{userid}/blogs/{blogid}</path>
  <segment name="userid">NH-101</segment>
  <segment name="blogid">@blogId</segment>
</rest>
{% endhighlight %}

Finally, you can apply query parameters in your url. The url *https://www.myapi.com/v2/users?sort=desc&limit=10* can be specified as

{% highlight xml %}
<rest base-url="https://www.myapi.com/v2/">
  <path>users</path>
  <parameter name="sort">desc</parameter>
  <parameter name="limit">10</parameter>
</rest>
{% endhighlight %}

## Alterations

You can also define an alteration to the result-set. For the moment, three kinds of alterations are supported by NBi:

* [projections](../resultset-alteration/#projections)
* [renamings](../resultset-alteration/#renamings)
* [extentions](../resultset-alteration/#extensions)
* [lookup for replacement](../resultset-alteration/#lookup-replaces)
* [filtering](../resultset-rows-count-advanced/#filters).
* [convertions](../resultset-alteration/#converts)
* [transformations](../transform-column/)
* [summarize](../resultset-alteration/#summarize)
* [reshaping](../resultset-alteration/#reshaping)

{% highlight xml %}
<result-set>
  <query>
    ...
  </query>
  <alteration>
    <filter ...>
  </alteration>
<result-set>
{% endhighlight %}

## Unavailability alternative

This feature is a limited preview and only works with xml-source for a few anticipated issues, including unavailability of the file to parse.

When specified the child element *if-unavailable* is providing an alternative if something is going wrong with the initial *result-set*. The alternative is defined as another *result-set*.

{% highlight xml %}
<result-set>
  <xml-source>
    ...
  </xml-source>
  <alteration>
    ...
  </alteration>
  <if-unavailable>
    <result-set>
      ...
    </result-set>
  </if-unavailable>
<result-set>
{% endhighlight %}