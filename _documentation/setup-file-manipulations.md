---
layout: documentation
title: Files' manipulations
prev_section: setup-data-manipulations-on-tables
next_section: setup-windows-services
permalink: /docs/setup-file-manipulations/
---
During the setup and cleanup stages, NBi offers the possibility to copy or remove files. This kind of manipulation could be useful before the run of an ETL.

* *file-delete*: this command let you delete a file. If the file is not existing, the command reports a success and doesn't try to delete the file.
* *file-copy*: this command let you make a copy of a file to another directory. If the source file doesn't exist, NBi will report an *ExternalDependencyNotFound* exception.

If the file is already existing on the destination then the file is overwritten. Finally, if the target directory doesn't exist, it will be created. For all of them, The xml attribute named *name* expects the name of the file (to copy or delete) and the xml attribute *path* is there to stipulate the full path of the file to delete or where the file will be copied.

{% highlight xml %}
<setup>
  <file-delete path="Temp\" name="foo.xls"/>
</setup>
{% endhighlight %}

In the case of the *copy* command, the attribute *source-path* tell us where is located the file to copy. For this command a simultaneous rename of the file isn't supported.  

{% highlight xml %}
<setup>
  <file-copy source-path="Backup\" path="Temp\" name="bar.xls"/>
</setup>
{% endhighlight %}
