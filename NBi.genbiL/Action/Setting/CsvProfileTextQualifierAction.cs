using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting
{
    public class CsvProfileTextQualifierAction : ICsvProfileAction
    {
        public char Value { get; set; }

        public CsvProfileTextQualifierAction(char value)
        {
            Value  = value;
        }

        public void Execute(GenerationState state)
        {
            var newProfile = state.Settings.CsvProfile;
            if (newProfile == null)
            {
                newProfile = new CsvProfileXml() { };
                state.Settings.CsvProfile = (newProfile);
            }
            newProfile.TextQualifier = Value;
        }

        public string Display => $"Create CSV profile setting named 'TextQualifier' and defining it to '{Value}'";
    }
}
