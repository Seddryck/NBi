using NBi.GenbiL.Stateful;
using NBi.Xml;
using NBi.Xml.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBi.GenbiL.Action.Suite
{
    public class SaveSuiteAction : ISuiteAction
    {
        public string Filename { get; protected set; }
        public StreamWriter StreamWriter { get; protected set; }

        public SaveSuiteAction(string filename)
        {
            Filename = filename;
        }

        public SaveSuiteAction(StreamWriter streamWriter)
        {
            StreamWriter = streamWriter;
        }
        
        public void Execute(GenerationState state)
        {
            foreach (var @default in state.Settings.Defaults)
            {
                switch (@default.Key)
                {
                    case DefaultType.SystemUnderTest:
                        state.Suite.Settings.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.SystemUnderTest, ConnectionString = @default.Value });
                        break;
                    case DefaultType.Assert:
                        state.Suite.Settings.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.Assert, ConnectionString = @default.Value });
                        break;
                    case DefaultType.SetupCleanup:
                        state.Suite.Settings.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.Decoration, ConnectionString = @default.Value });
                        break;
                    case DefaultType.Everywhere:
                        state.Suite.Settings.Defaults.Add(new DefaultXml { ApplyTo = SettingsXml.DefaultScope.Everywhere, ConnectionString = @default.Value });
                        break;
                 }   
            }

            foreach (var reference in state.Settings.References)
            {
                var referenceXml = new ReferenceXml();
                referenceXml.Name=reference.Key;
                referenceXml.ConnectionString=reference.Value;
                state.Suite.Settings.References.Add(referenceXml);
            }
                
            var manager = new XmlManager();
            if (StreamWriter==null)
                manager.Persist(Filename, state.Suite);
            else
                manager.Persist(StreamWriter, state.Suite);
        }

        public string Display
        {
            get
            {
                return string.Format("Saving the test-suite to '{0}'"
                    , Filename
                    );
            }
        }
    }
}
