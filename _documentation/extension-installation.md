---
layout: documentation
title: Install extensions
prev_section: extension-support
next_section: config-defaults-references
permalink: /docs/extension-installation/
---

## Manually

### Download and copy the binaries

If you're not using Visual Studio and package management to create your test-suite, you'll have to manually download the zip file of your extension. Once downloaded, open the archive and copy the content into the same folder than all the dlls of NBi itself (and not in a sub-folder).

### Register extension and binding redirections

You've to inform NBi that an extension has been installed. To achieve this, you'll have to edit your configuration file and add the section *extensions* to your file.

{% highlight xml %}
<configuration>
  <configSections>
    <section name="nbi" type="NBi.NUnit.Runtime.NBiSection, NBi.NUnit.Runtime"/>
  </configSections>
  <nbi testSuite="...">
    <extensions>
      <add assembly="NBi.Core.Gremlin"/>
    </extensions>
  </nbi>
</configuration>
{% endhighlight %}

The name of the assembly is the name of the main dll without the file extension.

Binding redirection is the only way to explain to the .Net Framework that you've different versions of a same dll. When developping or deploying extensions it's usually a problem that you'll meet. In .Net, if two assemblies have dependencies to the same dll but different versions, you'll have to authorize the framework to use the only version contained in the folder. This is exactly what is done in the section *runtime* of your configuration file. This binding redirection explains that any lookup to the dll *Newtonsoft.Json* from version 0.0 to 10.0 can be redirected to version 10.0

{% highlight xml %}
<runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-10.0.0.0" newVersion="10.0.0.0" />
      </dependentAssembly>
    </assemblyBinding>
</runtime>
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