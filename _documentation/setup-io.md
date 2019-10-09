---
layout: documentation
title: Files' manipulations
prev_section: setup-data-engineering
next_section: setup-process
permalink: /docs/setup-io/
---
During the setup and cleanup stages, NBi offers the possibility to copy or remove files. This kind of manipulation could be useful before the run of an ETL.

All the attributes, enlisted here under, accept [literal values](../primitive-scalar/#literal), and [variables](../primitive-scalar/#reference-to-a-variable) including [formatting](../primitive-scalar/#formatting) and [inline transformations](../primitive-scalar/#inline-transformations).

## Basic copy and delete

* *file-delete*: this command let you delete a file. If the file is not existing, the command reports a success and doesn't try to delete the file.
* *file-copy*: this command let you make a copy of a file to another directory. If the source file doesn't exist, NBi will report an *ExternalDependencyNotFound* exception.

For all of them, The xml attribute named *name* expects the name of the file (to copy or delete) and the xml attribute *path* is there to stipulate the full path of the file to delete or where the file will be copied.

Specifically for *file-copy*, if the file is already existing on the destination then this file will be overwritten. If the target directory doesn't exist, it will be created.

{% highlight xml %}
<setup>
  <file-delete path="Temp\" name="foo.xls"/>
</setup>
{% endhighlight %}

In the case of the *copy* command, the attribute *source-path* tell us where is located the file to copy. For this command, a simultaneous rename of the file isn't supported.  

{% highlight xml %}
<setup>
  <file-copy source-path="Backup\" path="Temp\" name="bar.xls"/>
</setup>
{% endhighlight %}

## Advanced features

In some cases, you'll need to copy or delete a bunch of files. For each of the *copy* and *delete* commands, NBi provides two alternatives to define the set of files to be copied or deleted.

### Filename pattern

In this variation, you're defining the file to copy or delete by using the respective commands *file-copy-pattern* and *file-delete-pattern*. The xml attribute *pattern* specifies the pattern that a file must match to be copied (or deleted).

The example here under will delete all the files starting by *foo* and ending by two characters preceded by a dash.

{% highlight xml %}
<setup>
  <file-delete-pattern path="Backup\" pattern="foo*-??.xls"/>
</setup>
{% endhighlight %}

If no file has the extension provided in the command, no file will be copied or deleted but it won't be considered as a failure of the task.

### Files extension

In this variation, you're defining the file to copy or delete by using the respective commands *file-copy-extension* and *file-delete-extension*. The xml attribute *extension* specifies the extention that a file must match to be copied (or deleted). An extension must be defined including the initial dot, so you should specifies *.txt* and not *txt*. It's possible to specifies several extensions that must be copied or deleted by seperating them by a semi-column. The following attribute's value *.txt;.csv* will select all the files with an extension *.txt* or *.csv*.

The example here under will delete all the files with an extension *.txt* or *.csv*.

{% highlight xml %}
<setup>
  <file-delete-extension path="Backup\" extension=".txt;.csv"/>
</setup>
{% endhighlight %}

If no file has the extension provided in the command, no file will be copied or deleted but it won't be considered as a failure of the task.