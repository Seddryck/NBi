using System;
using System.IO;
using System.Xml;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.Command
{
	class AutoIndentCommand : CommandBase
	{
		private readonly DocumentPresenterBase document;

		public AutoIndentCommand(DocumentPresenterBase document)
		{
			this.document = document;
		}

		public override string Name
		{
			get { return "AutoIndent"; }
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			//print request xml
			var doc = new XmlDocument();
			var stringWriter = new StringWriter();

			//get your document
			try
			{
				doc.LoadXml(this.document.Text);
			}
			catch (Exception)
			{
				return;
			}

			//create reader and writer
			var xmlReader = new XmlNodeReader(doc);

			//set formatting options
			var xmlWriter = new XmlTextWriter(stringWriter)
			                	{
			                		Formatting = Formatting.Indented, 
			                		Indentation = 3, 
			                		IndentChar = ' '
			                	};

			//write the document formatted
			xmlWriter.WriteNode(xmlReader, true);

			this.document.Text = stringWriter.ToString();
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = string.IsNullOrEmpty(this.document.Text);
		}
	}
}