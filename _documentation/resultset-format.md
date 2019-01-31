---
layout: documentation
title: Cell's format in a result-set
prev_section: resultset-rows-uniqueness
next_section: transform-column
permalink: /docs/resultset-format/
---
Other features offer the opportunity to test the values returned by a query and compare them to expected value. This feature is not working on the value but on the format. This is only relevant for SSAS because this application see the value and the format as two different matters.
Suppose that you've a measure returning an amount in Euro. Other features of NBi will check that this value is in the correct interval or greater than and so on. But nothing will confirm that the value is rendered as expected to the end-user, with a symbol € and a dot as decimal separator. If you want to perform this test you'll need to use the _matchPattern_ element as an assertion.

## System-under-test

The system-under-test is a query. NBi will only check the first column returned by the query.

## Assert

The assertion consists of an xml element _matchPattern_ embedding another xml element defining the expected format.
{% highlight xml %}
<assert>  
  <matchPattern>  
    ...
  </matchPattern>  
</assert>  
{% endhighlight %}

This xml element _matchPattern_ allows the usage of three different children explained here under.

### Number format

The *numeric-format* xml element will let the user specify which character will be used as decimal separator (attribute *decimal-separator*), group separator (attribute *group-separator*) and how many figures are expected after the decimal symbol (attribute *decimal-digits*).

The following specification will validate the formatted value of '1,152.32' but not '1152,32' neither '1.125,320'
{% highlight xml %}
<assert>  
  <matchPattern>  
    <numeric-format  
      decimal-digits="2"  
      decimal-separator="."  
      group-separator=","
    />  
  </matchPattern>  
</assert>  
{% endhighlight %}

### Currency format

The *currency-format* xml element is an extension of the previously defined *number-format*. In addition to the previous attributes, you must also specify the symbol for the currency (attribute *currency-symbol*) and if this symbol is positioned before or after the value and with or without a space (attribute *currency-pattern*).
{% highlight xml %}
<assert>  
  <matchPattern>  
    <currency-format  
      currency-pattern="$n"  
      currency-symbol="$"  
      decimal-digits="2"  
      decimal-separator="."  
      group-separator=","
    />  
  </matchPattern>  
</assert>  
{% endhighlight %}

### Regular expression

If the two options above don't cover your needs, you can also make usage of a regular expression. In this case you'll need to use the xml element *regex* and assign to it the regular expression.
{% highlight xml %}
<assert>  
  <matchPattern>  
    <regex>^\$?[0-9]{1,3}(?:,?[0-9]{3})\*\.[0-9]{2}$</regex>
  </matchPattern>  
</assert>
{% endhighlight %}

## Reference a format in multiple tests

It's always useful to define at one place the expected formats. For this you can create _references_ in the _settings_ and define some formats. In teh sample bellow, we're creating a reference named 'Standard'
{% highlight xml %}
<settings>
    <default apply-to="system-under-test">
      <connectionString>Provider=MSOLAP.4;Data Source=(local)\SQL2012;Initial Catalog='Adventure Works DW 2012';localeidentifier=1049</connectionString>
    </default>
    <reference name="Standard">
      <currency-format
          currency-pattern="$n"
          currency-symbol="$"
          decimal-digits="2"
          decimal-separator="."
          group-separator=","
      />
    </reference>
</settings>
{% endhighlight %}

Later in your test, you'll only need to link the expected format to this reference. Here under, we're linking our assertion to the reference 'Standard' previously defined.
{% highlight xml %}
<assert>  
  <matchPattern>
    <currency-format
       ref="Standard"
    />
  </matchPattern>
</assert>
{% endhighlight %}

## Full Example

This sample illustrate a query and the corresponding assertion for the format of expected result.
{% highlight xml %}
<test name="'Reseller Order Count' by year" uid="0001">  
  <system-under-test>  
    <execution>  
      <query>  
        SELECT  
          [Measures].[Amount] ON 0,  
          non empty([Date].[Calendar].[Calendar Year]) ON 1  
        FROM  
          [Adventure Works]  
       </query>  
    </execution>  
  </system-under-test>  
  <assert>  
    <matchPattern>  
      <currency-format  
        currency-pattern="$n"  
        currency-symbol="$"  
        decimal-digits="2"  
        decimal-separator="."  
        group-separator=","
      />  
    </matchPattern>  
  </assert>  
</test>
{% endhighlight %}
