---
layout: automation
title: Build a test-suite
prev_section: comments
next_section: cicd-run-azuredevops
permalink: /automation/cicd-build-azuredevops/
---
Using Azure DevOps Pipelines, it's possible to build your test-suite based on your genbiL file and other supporting files. This create a world of opportunity in terms of versioning of your test-suites.

In the following step-by-step example, we'll connect to a directory at GitHub and build the test-suite defined in a file named *dictionary.genbiL* using the latest release of *Genbi.exe* available on [GitHub releases](https://github.com/seddryck/nbi/releases/latest).

The GitHub repository containing the fake test-suite is available at <https://github.com/seddryck/nbi.cicd.build> and the Azure DevOps Pipelines is available at <https://seddryck.visualstudio.com/NBi.Cicd.Build/_build>.

## Setting up a new pipeline on Azure DevOps

We assume that you already have an account at Azure DevOps, if it's not the case many tutorials to create an account are available online.

Once you're connected to your Azure DevOps account or organization, create a new project. In this case we'll name it *"*NBi.Cicd.Build*.

After this creation, you'll have to create a pipeline. The first step is to use the navigation pane to go to *pipelines/build* and click on new pipeline.

![create-pipeline]({{ site.baseurl }}/img/automation/create-pipeline.png)

 You'll have first to specify that you want to connect to a repository on GitHub

![github-repository-1]({{ site.baseurl }}/img/automation/github-repository-1.png)

And then to select the repository

![github-repository-2]({{ site.baseurl }}/img/automation/github-repository-2.png)

During the next step, you'll have to specify that you want to create a new pipeline from scratch by selecting the option *Starter Pipeline*.

![configure-pipeline]({{ site.baseurl }}/img/automation/configure-pipeline.png)

## Configuration of the pipeline

### Switch to windows based agent

*genbi.exe* only runs on windows, the first step will be to change the *Ubuntu* Virtual Machine of the build agent to a *Windows* version. To achieve this edit the *vmImage* information and replace by ```windows-latest```.

![pool-vmimage]({{ site.baseurl }}/img/automation/pool-vmimage.png)

{% highlight yaml %}
pool:
  vmImage: 'windows-latest'
{% endhighlight %}

### Download and install genbi.exe

To download *genbi.exe*, you can rely on GitHub releases. For each major version a zip file including *genbi.exe* and all its libraries is available on the [GitHub releases website](https://github.com/seddryck/nbi/releases). To quickly identify the latest release, you just have to append */latest* at the base url: <https://github.com/seddryck/nbi/releases/latest>.

![github-releases]({{ site.baseurl }}/img/automation/github-releases.png)

You can also access this information through the GitHub API at the following url: <https://api.github.com/repos/seddryck/nbi/releases/latest>.

The following powershell script, let you parse the content of the API response and retrieve the tag name corresponding to the latest version and the public url to download the binaries. ast line of the script download the file and persist it on the *artifcats directory*.

{% highlight powershell %}
$latestRelease = Invoke-WebRequest https://api.github.com/repos/seddryck/nbi/releases/latest -Headers{"Accept"="application/json"}
$json = $latestRelease.Content | ConvertFrom-Json
write-host 'latest version:' $json.tag_name
$url = $json.assets | ? { $_.Name -Match "Genbi" }  | % browser_download_url
write-host 'url:' $url
Invoke-WebRequest -Uri $url -OutFile "$env:SYSTEM_ARTIFACTSDIRECTORY\genbi.zip"
{% endhighlight %}

You can execute this script in your pipeline by adding a *Run Inline Powershell* task. Search for a *powershell* task and select the task named *Run Inline Powershell*.

![task-run-inline-powershell]({{ site.baseurl }}/img/automation/task-run-inline-powershell.png)

Then copy paste the script and click on *add*

![add-script]({{ site.baseurl }}/img/automation/add-script.png)

Once the file has been download, you'll need to extract it. You can achieve this by using a task *Extract files*.  Search for a *Extract* task and select the task named *Extract files*.

![task-extract-files]({{ site.baseurl }}/img/automation/task-extract-files.png)

Fill the different parameters of the task. don't forget to clean the folder before unzipping and to double the backslashes.

![extract-files-params]({{ site.baseurl }}/img/automation/extract-files-params.png)

### Run genbi.exe

Once downloaded and unzipped, *genbi.exe* is ready to use. You can call it through a command line passing in parameter the path to your genbiL file and the option ```-q``` to avoid interactive pop-ups.

Search for a *Command* task and select the task named *Command line*.

![task-command-line]({{ site.baseurl }}/img/automation/task-command-line.png)

then write the following script

{% highlight yaml %}
$(System.ArtifactsDirectory)\\Genbi\\genbi.exe $(System.DefaultWorkingDirectory)\\dictionary.genbiL -q
{% endhighlight %}

![add-cmd-script]({{ site.baseurl }}/img/automation/add-cmd-script.png)

### Promote the output as artifact

Now that the test-suite has been created, we'll need to promote it as an artefact. A first task will copy the test-suite to the *staging directory* and a second will transform the content of the *staging directory* to a zip file that will be available as an output of the pipeline (artefact).

Select the task *copy files*

![task-copy-files]({{ site.baseurl }}/img/automation/task-copy-files.png)

and fill the parameters. Pay attention to clean the folder before copying the nex test-suites.

![copy-files-params]({{ site.baseurl }}/img/automation/copy-files-params.png)

Once, the test-suite is available in the *staging directory*, we can publish the content of this directory as an artifact. To do this, use the task *publish build artifacts*

![task-publish-build-artifacts]({{ site.baseurl }}/img/automation/task-publish-build-artifacts.png)

and don't forget to rename this artifact *test-suite*

![publish-build-artifacts-params]({{ site.baseurl }}/img/automation/publish-build-artifacts-params.png)

### Trigger the pipeline

Once the pipeline defined, don't forget to save it. This save will trigger the upload of a file named *
azure-pipelines.yml* in your GitHub repository. Due to the change in this repository, it will also trigger the pipeline on Azure DevOps. 

![save-run]({{ site.baseurl }}/img/automation/save-run.png)

If later you update any file in this directory, it will also trigger the pipeline.

### Download artifact

Once the pipeline has completed, the artifact is available and can be downloaded or consummed by another pipeline.

![download-artifact]({{ site.baseurl }}/img/automation/download-artifact.png)

The full script of the [pipeline](https://github.com/Seddryck/NBi.Cicd.Build/blob/master/azure-pipelines.yml) is available on GitHub and the result of this [pipeline execution](https://seddryck.visualstudio.com/NBi.Cicd.Build/_build) is also visible on Azure DevOps.