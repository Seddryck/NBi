using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace NBi.UI.Configuration
{
    public class Project
    {
        private static DirectoryCollection _directories;
        public static DirectoryCollection Directories
        {
            get
            {
                if (_directories == null)
                    _directories = new DirectoryCollection();
                return _directories;
            }
        }

        private static ConnectionStringCollection _connectionStrings;
        public static ConnectionStringCollection ConnectionStrings
        {
            get
            {
                if (_connectionStrings == null)
                    _connectionStrings = new ConnectionStringCollection();
                return _connectionStrings;
            }
        }

        public static void Load(string filename)
        {

            var nbiFile = new XmlDocument();
            nbiFile.Load(filename);

            var dirsNode = nbiFile.GetElementsByTagName("directories")[0];
            if (dirsNode != null)
            {
                if (dirsNode.Attributes["root"] != null)
                    Directories.Root = dirsNode.Attributes["root"].Value;

                var dirNodes = nbiFile.GetElementsByTagName("directory");
                foreach (XmlNode dirNode in dirNodes)
                {
                    var key = BuildDirectoryKey(dirNode.Attributes["key"].Value);
                    Directories[key] = new Directory(Directories);

                    if (dirNode.Attributes["path"] != null)
                        Directories[key].Path = dirNode.Attributes["path"].Value;
                    if (dirNode.Attributes["file"] != null)
                        Directories[key].File = dirNode.Attributes["file"].Value;
                }
            }

            var cssNode = nbiFile.GetElementsByTagName("connectionStrings")[0];
            if (cssNode != null)
            {

                foreach (XmlNode csNode in cssNode.ChildNodes)
                {
                    var key = BuildConnectionStringKey(csNode.Name, csNode.Attributes["key"].Value);
                    ConnectionStrings[key] = new ConnectionString();

                    if (csNode.InnerXml != null)
                        ConnectionStrings[key].Value = csNode.InnerXml;
                }
            }
        }

        protected static DirectoryCollection.DirectoryType BuildDirectoryKey(string key)
        {
            DirectoryCollection.DirectoryType result;
            if (Enum.TryParse<DirectoryCollection.DirectoryType>(key, true, out result))
                return result;

            throw new NotImplementedException();
        }

        protected static ConnectionStringCollection.ConnectionDefinition BuildConnectionStringKey(string tag, string key)
        {
            ConnectionStringCollection.ConnectionType typeResult;
            if (!Enum.TryParse<ConnectionStringCollection.ConnectionType>(key, true, out typeResult))
                throw new NotImplementedException();

            ConnectionStringCollection.ConnectionClass classResult;
            if (!Enum.TryParse<ConnectionStringCollection.ConnectionClass>(tag, true, out classResult))
                throw new NotImplementedException();

            return new ConnectionStringCollection.ConnectionDefinition(classResult, typeResult); 
        }

        public static void Save(string filename)
        {
            var nbiFile = new XmlDocument();

            //Create an XML declaration. 
            XmlDeclaration xmldecl = nbiFile.CreateXmlDeclaration("1.0", "utf-8", null);

            //Add the new node to the document.
            XmlElement root = nbiFile.DocumentElement;
            nbiFile.InsertBefore(xmldecl, root);

            //Add the Project root node
            var prj = nbiFile.CreateNode(XmlNodeType.Element, "project", "");
            nbiFile.AppendChild(prj);

            XmlNode dirs = nbiFile.CreateNode(XmlNodeType.Element, "directories","");
            prj.AppendChild(dirs);
            
            if (!String.IsNullOrEmpty(Directories.Root))
            {
                dirs.Attributes.SetNamedItem(nbiFile.CreateNode(XmlNodeType.Attribute, "root", ""));
                dirs.Attributes["root"].Value=Directories.Root;
            }

            foreach (System.Collections.Generic.KeyValuePair<DirectoryCollection.DirectoryType, Directory> directoryKVP in Directories)
            {
                var directory = directoryKVP.Value;
                XmlNode dir = nbiFile.CreateNode(XmlNodeType.Element, "directory","");
                dir.Attributes.SetNamedItem(nbiFile.CreateNode(XmlNodeType.Attribute, "key", ""));
                dir.Attributes["key"].Value = Enum.GetName(typeof(DirectoryCollection.DirectoryType), directoryKVP.Key);
                dirs.AppendChild(dir);
                if (!String.IsNullOrEmpty(directory.Path))
                {
                    dir.Attributes.SetNamedItem(nbiFile.CreateNode(XmlNodeType.Attribute, "path", ""));
                    dir.Attributes["path"].Value = directory.Path;
                }
                if (!String.IsNullOrEmpty(directory.File))
                {
                    dir.Attributes.SetNamedItem(nbiFile.CreateNode(XmlNodeType.Attribute, "file", ""));
                    dir.Attributes["file"].Value = directory.File;
                }
            }

            XmlNode cssNode = nbiFile.CreateNode(XmlNodeType.Element, "connectionStrings", "");
            prj.AppendChild(cssNode);

            foreach (System.Collections.Generic.KeyValuePair<ConnectionStringCollection.ConnectionDefinition, ConnectionString> csKVP in ConnectionStrings)
            {
                var cs = csKVP.Value;
                XmlNode csNode = nbiFile.CreateNode(XmlNodeType.Element, Enum.GetName(typeof(ConnectionStringCollection.ConnectionClass), csKVP.Key.Class).ToLower(), "");
                csNode.Attributes.SetNamedItem(nbiFile.CreateNode(XmlNodeType.Attribute, "key", ""));
                csNode.Attributes["key"].Value = Enum.GetName(typeof(ConnectionStringCollection.ConnectionType), csKVP.Key.Type);
                csNode.InnerXml = cs.Value;
                cssNode.AppendChild(csNode);
            }
            using(var wr = new StreamWriter(filename, false, System.Text.Encoding.UTF8))
	        {
                nbiFile.Save(wr);
	        }
           
        }
    }
}
