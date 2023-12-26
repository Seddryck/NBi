using System.Reflection;
using System.Runtime.CompilerServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany("NBi Team - Cédric L. Charlier")]
[assembly: AssemblyProduct("NBi")]
[assembly: AssemblyCopyright("Copyright © Cédric L. Charlier 2012-2021")]
[assembly: AssemblyDescription("NBi is a testing framework (add-on to NUnit) for Microsoft Business Intelligence platform and Data Access. The main goal of this framework is to let users create tests with a declarative approach based on an Xml syntax. By the means of NBi, you don't need to develop C# code to specify your tests! Either, you don't need Visual Studio to compile your test suite. Just create an Xml file and let the framework interpret it and play your tests. The framework is designed as an add-on of NUnit but with the possibility to port it easily to other testing frameworks.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

//Reference the testing class to ensure access to internal members
[assembly: InternalsVisibleTo("NBi.Testing")]
[assembly: InternalsVisibleTo("NBi.Core.Testing")]
[assembly: InternalsVisibleTo("NBi.Core.Testing")]
[assembly: InternalsVisibleTo("NBi.Framework.Testing")]
[assembly: InternalsVisibleTo("NBi.GenbiL.Testing")]
[assembly: InternalsVisibleTo("NBi.Xml.Testing")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.23")]
[assembly: AssemblyFileVersion("1.23")]
[assembly: AssemblyInformationalVersion("1.23")]
