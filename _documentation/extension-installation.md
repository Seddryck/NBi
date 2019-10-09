---
layout: documentation
title: Install extensions
prev_section: extension-support
next_section: extension-flatfile
permalink: /docs/extension-installation/
---

## Manually

If you're not using Visual Studio and the package management tool Nuget to create your test-suite, 

### Register the extension

You've to inform NBi that an extension is installed. To achieve this, you'll have to edit your *configuration file* and add the section *extensions* to the *nbi* section of your file.

{% highlight xml %}
<configuration>
  <nbi testSuite="...">
    <extensions>
      <add assembly="NBi.Core.Gremlin"/>
    </extensions>
  </nbi>
</configuration>
{% endhighlight %}

The name of the assembly is the name of the main dll of the extension without the file extension.

### Deployment of the binaries for the extension

You must download the zip file of your extension and select one of the two options here under. The second option sounds cleaner but requires some additional configuration.

* In the same folder than NBi: open the extension's archive and copy the content into the same folder than all the dlls of NBi itself.
* In a sub-folder:  open the extension's archive and copy the content into a sub-folder of the folder where NBi is deployed. You can name it as the name of this extension or not, up-to-you. After that, you'll have to edit your NUnit project file and change the following information: The *binPathType* should be set to *Manual* and the *binPath* should be set to *folder;folder\sub-folder*. The semi-column is needed to seperate two paths, if you deploy more than one extension, add additional paths separated by semi-columns.

In this example, NBi is deployed in a folder named *Framework* and the extension for Gremlin is a sub-folder named *NBi.Core.Gremlin*

{% highlight xml %}
<NUnitProject>
  <Settings activeconfig="Default" processModel="Default" domainUsage="Default" />
  <Config
    name="Default"
    binpath="Framework;Framework\NBi.Core.Gremlin"
    binpathtype="Manual"
    appbase="..\"
    configfile="FoF\FoF.config">
    <assembly path="Framework\NBi.NUnit.Runtime.dll" />
  </Config>
</NUnitProject>
{% endhighlight %}

Usage of a sub-folder **doesn't overcome** the need to specify binding redirections.

### Binding redirections

Binding redirection is the only way to explain to the .Net Framework that you've different versions of a same dll. When developping or deploying extensions it's usually a problem that you'll meet. In .Net, if two assemblies have dependencies to the same dll but different versions, you'll have to authorize the framework to use the only version contained in the folder. This is exactly what is done in the section *runtime* of your configuration file. This binding redirection explains that any lookup to the dll *Newtonsoft.Json* from version 0.0 to 10.0 can be redirected to version 10.0

{% highlight xml %}
<configuration>
  <configSections>
    <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
  </configSections>
  <nbi testSuite="..."/>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-10.0.0.0" newVersion="10.0.0.0" />
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
</configuration>
{% endhighlight %}

The binding redirections that you'll have to stipulate in your configuration file depends of the extension. You'll find them in the config file associated to the extension. If you extension is *NBi.Core.Gremlin* then the name of this file should be *NBi.Core.Gremlin.dll.config*. Copy the content of the section *runtime* and paste it into your test-suite configuration file.

If you want to load more than one extension, you'll probably have to merge the different config files.

When binding redirections is not correctly setup, you'll receive message such as

{% highlight xml %}
An exception of type 'System.IO.FileLoadException' occurred in System.Net.Http.Formatting.dll but was not handled in user code
Additional information: Could not load file or assembly 'Newtonsoft.Json, Version=6.0.2.16931, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed' or one of its dependencies. The located assembly's manifest definition does not match the assembly reference. (Exception from HRESULT: 0x80131040)
{% endhighlight %}

You'll have to check the name of the dll not found, check that this dll is in your NBi framework folder and check that there is a binding redirection for this dll in your test-suite configuration file.

## Visual Studio

Install the package NBi.VisualStudio to correctly setup the environment.

{% highlight ps1 %}
Install-Package NBi.VisualStudio
{% endhighlight %}

Install the package NBi.Gremlin to add the extension

{% highlight ps1 %}
Install-Package NBi.Gremlin
{% endhighlight %}

Then, you'll have to fix the binding redirects and add them to the *TestSuite.config* file ... it's not always trivial.