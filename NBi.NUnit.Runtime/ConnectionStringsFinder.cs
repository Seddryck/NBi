using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NBi.Core;

namespace NBi.NUnit.Runtime
{
	public class ConnectionStringsFinder
	{
		protected internal virtual NameValueCollection Find()
		{
			var list = new NameValueCollection();
			string configFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
			//Try to find a config file, if existing take the path inside for the TestSuite
			if (File.Exists(configFile))
			{
				//line bellow to avoid .Net framework bug: http://support.microsoft.com/kb/2580188/en-us
				var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

				if (configuration.ConnectionStrings != null)
				{
					Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Section 'connectionStrings' found."));
					if (!string.IsNullOrEmpty(configuration.ConnectionStrings.SectionInformation.ConfigSource))
                        Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Section 'connectionStrings' overriden by '{1}'.", configuration.ConnectionStrings.SectionInformation.ConfigSource));

					foreach (ConnectionStringSettings css in configuration.ConnectionStrings.ConnectionStrings)
					{
						list.Add(css.Name, css.ConnectionString);
						Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Connection string named '{0}' loaded with value '{1}'", css.Name, css.ConnectionString));
					}
				}
					
						
			}
			return list;
		}
	}
}
