using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NBi.Xml
{
    public class XmlManager
    {
        public TestSuiteXml TestSuite {get; protected set;}
        protected bool _isValid; 

        public XmlManager() { }

        public void Load(string filename)
        {
            XmlTextReader xmlTextReader = new XmlTextReader(filename);

            if (!this.Validate(xmlTextReader))
                throw new ArgumentException("The test suite is not valid. Check with the XSD");

            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new XmlSerializer(typeof(TestSuiteXml));
                        
            using (StreamReader reader = new StreamReader(filename))
            {
                // Use the Deserialize method to restore the object's state.
                TestSuite = (TestSuiteXml)serializer.Deserialize(reader);
            }
        }

        protected bool Validate(XmlTextReader xmlTextReader)
        {
            //We pass the xmltextreader into the xmlvalidatingreader
            //This will validate the xml doc with the schema file
            XmlValidatingReader validator = new XmlValidatingReader(xmlTextReader);

            // Set the validation event handler
            validator.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
            //make sure to reset the success var
            _isValid = true; 
            
            // Read XML data
            while (validator.Read()) { }

            //Close the validator.
            validator.Close();

            //The validationeventhandler is the only thing that would set 
            //m_Success to false

            return _isValid;
        }

        private void ValidationCallBack(Object sender, ValidationEventArgs args)
        {
            //This is only called on error
            _isValid = false; //Validation failed
        }
    }
}
