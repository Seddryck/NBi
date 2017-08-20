---
layout: documentation
title: CSV profile
prev_section: config-settings-external-file
next_section: config-profile-failure-report
permalink: /docs/config-profile-csv/
---

## Separators

A CSV file consists of any number of records, separated by line breaks of some kind; each record consists of fields, separated by some other character or string, most commonly a literal comma, semi-column or tab. A general standard for the CSV file format does not exist. The CSV profile offers the opportunity to specify the characters used to delimit fields and records.

By default (if nothing else is specified), the CSV profile is defined with a field separator set to a semi-column (;) and a record separator set to carriage return line feed (\r\n).

This value by default can be overridden in the xml element *settings* by adding a xml element named *csv-profile* . This element is requiring two xml attributes named *field-separator* and *record-separator*.

{% highlight xml %}
<settings>
   <csv-profile field-separator="," record-separator="#"/>
</settings>
{% endhighlight %}

Note that the field-separator must be limited to 1 character but the record separator can have more characters. For the moment, the escape and quoting characters are not implemented.

The xml element automatically translates a few special values to their corresponding caracters.

* Tab is translated to \t (tabulation) only for field-separator
* Cr is translated to \r (carriage return) only for record-separator
* Lf is translated to \n (line feed) only for record-separator

In this example, a tabulation is specified as field-separator and a carriage return line feed for record-separator.
{% highlight xml %}
<settings>
   <csv-profile field-separator="Tab" record-separator="CrLf"/>
</settings>
{% endhighlight %}

## First row and missing/empty cells

By default, the first row is considered to be a row of data. If you've headers in your CSV files, you should activate the setting:
{% highlight xml %}
<settings>
   <csv-profile first-row-header="true" />
</settings>
{% endhighlight %}

You can also specify the behaviour when there is no value defined for a cell or when this cell is missing (missing field-separators).
{% highlight xml %}
<settings>
   <csv-profile
       empty-cell="NULL"
       missing-cell="NULL"
   />
</settings>
{% endhighlight %}