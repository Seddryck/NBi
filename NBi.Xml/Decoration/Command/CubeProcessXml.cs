using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Xml.Settings;
using System.Xml.Serialization;
using NBi.Core.Analysis.Process;

namespace NBi.Xml.Decoration.Command
{
    public class CubeProcessXml : DecorationCommandXml, ICubeProcess
    {
        [XmlAttribute("connectionString")]
        public string SpecificConnectionString { get; set; }

        [XmlAttribute("database")]
        public string Database { get; set; }

        [XmlAttribute("cube")]
        public string Cube { get; set; }

        [XmlElement("dimension", Order = 1)]
        public List<DimensionProcessXml> InternalDimensions { get; set; }

        [XmlIgnore()]
        public IEnumerable<IDimensionProcess> Dimensions 
        {
            get
            {
                return InternalDimensions.Cast<IDimensionProcess>();
            }
        }

        [XmlElement("measure-group", Order = 2)]
        public List<MeasureGroupProcessXml> InternalMeasureGroups  { get; set; }

        [XmlIgnore()]
        public IEnumerable<IMeasureGroupProcess> MeasureGroups
        {
            get
            {
                return InternalMeasureGroups.Cast<IMeasureGroupProcess>();
            }
        }

        [XmlElement("partition", Order = 3)]
        public List<PartitionProcessXml> InternalPartitions { get; set; }

        [XmlIgnore()]
        public IEnumerable<IPartitionProcess> Partitions
        {
            get
            {
                return InternalPartitions.Cast<IPartitionProcess>();
            }
        }

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
                return (Dimensions.Count() == 0 && MeasureGroups.Count() == 0);
            }
        }

        public CubeProcessXml()
        {
            InternalDimensions = new List<DimensionProcessXml>();
            InternalMeasureGroups = new List<MeasureGroupProcessXml>();
            InternalPartitions = new List<PartitionProcessXml>();
        }
    }
}
