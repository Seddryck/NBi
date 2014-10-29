﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using NBi.Core.Etl;
using NBi.Xml.Items;
using System.IO;
using NBi.Core.Batch;
using NBi.Xml.Settings;

namespace NBi.Xml.Decoration.Command
{
    public class SqlRunXml : DecorationCommandXml, IBatchRunCommand
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("path")]
        public string InternalPath { get; set; }

        [XmlIgnore]
        public string FullPath
        {
            get
            {
                var fullPath = string.Empty;
                if (!Path.IsPathRooted(InternalPath) || string.IsNullOrEmpty(Settings.BasePath))
                    fullPath = InternalPath + Name;
                else
                    fullPath = Settings.BasePath + InternalPath + Name;

                return fullPath;
            }
        }

        [XmlAttribute("connectionString")]
        public string SpecificConnectionString { get; set; }

        [XmlIgnore]
        public string ConnectionString
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(SpecificConnectionString))
                    return SpecificConnectionString;
                if (Settings != null && Settings.GetDefault(SettingsXml.DefaultScope.Decoration) != null)
                    return Settings.GetDefault(SettingsXml.DefaultScope.Decoration).ConnectionString;
                return string.Empty;
            }
        }

        public SqlRunXml()
        {
          
        }
    }
}
