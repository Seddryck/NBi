using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Xml.Settings;
using System.Xml.Serialization;

namespace NBi.Xml.Decoration.Command
{
    public class CubeProcessXml : DecorationCommandXml
    {
        [XmlAttribute("connectionString")]
        public string SpecificConnectionString { get; set; }

        [XmlAttribute("cube")]
        public string Cube { get; set; }

        [XmlElement("dimension", Order = 1)]
        public List<DimensionProcessXml> Dimensions { get; set; }

        [XmlElement("measure-group", Order = 2)]
        public List<MeasureGroupProcessXml> MeasureGroups  { get; set; }

        [XmlIgnore]
        public string ConnectionString
        {
            get
            {
                //Return the Specific ConnectionString, if specified
                if (!String.IsNullOrWhiteSpace(SpecificConnectionString))
                    return SpecificConnectionString;

                //Return the Olap ConnectionString of the default element, if specified
                if (Settings != null && Settings.GetDefault(SettingsXml.DefaultScope.Decoration) != null)
                    if (! string.IsNullOrEmpty(Settings.GetDefault(SettingsXml.DefaultScope.Decoration).OlapConnectionString))
                        return Settings.GetDefault(SettingsXml.DefaultScope.Decoration).OlapConnectionString;
                
                //Return the Default ConnectionString, if specified
                if (Settings != null && Settings.GetDefault(SettingsXml.DefaultScope.Decoration) != null)
                    return Settings.GetDefault(SettingsXml.DefaultScope.Decoration).ConnectionString;
                
                //return empty
                return string.Empty;
            }
        }

        [XmlIgnore]
        public bool IsWholeCube 
        { 
            get
            {
                return (Dimensions.Count == 0 && MeasureGroups.Count == 0);
            }
        }

        public CubeProcessXml()
        {
            Dimensions = new List<DimensionProcessXml>();
            MeasureGroups = new List<MeasureGroupProcessXml>();
        }
    }
}
