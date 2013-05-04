using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Antlr4.StringTemplate;

namespace NBi.Service.RunnerConfig
{
    internal abstract class AbstractRunnerConfigBuilder : IRunnerConfigBuilder
    {
        protected IFilePersister filePersister;
        
        private string ConfigTemplateFilename { get; set; }
        private string RunnerProjectTemplateFilename { get; set; }
        public string ConfigFullPath { get; private set; }
        public string RunnerProjectFullPath { get; private set; }
        
        protected string Root {get; set;}
        protected string Framework {get; set;}
        protected string TestSuite { get; set; }
        protected string BasePath {get; set;}
        protected string Filename {get; set;}

        private Dictionary<string, string> Dico { get; set; }

        public AbstractRunnerConfigBuilder(string configTemplateFilename, string runnerProjectTemplateFilename)
        {
            ConfigTemplateFilename = configTemplateFilename;
            RunnerProjectTemplateFilename = runnerProjectTemplateFilename;
        }

        public void Build(string basePath, string rootPath, string frameworkPath, string testSuitePath, string testSuiteFilename)
        {            
            BasePath = basePath;
            Root = rootPath;
            Framework = frameworkPath;
            TestSuite = testSuitePath;
            Filename = testSuiteFilename;
            Dico = new Dictionary<string, string>();

            ConfigFullPath = CalculateConfigFullPath();
            RunnerProjectFullPath = CalculateRunnerProjectFullPath();

            OnPreCalculateDico();
            CalculateDico();
            OnPostCalculateDico();

            BuildRunnerProjectFile();
            BuildConfigFile();
            CopyRuntimeFile();
        }


        protected virtual string CalculateRunnerProjectFullPath()
        {
            return BasePath + TestSuite + Path.GetFileNameWithoutExtension(this.Filename);
        }

        protected abstract string CalculateConfigFullPath();

        private void CalculateDico()
        {
            AddToDico("TestSuitFilename", Filename);
            AddToDico("RootRelative", Root);
            AddToDico("FrameworkRelative", Framework);
            AddToDico("TestSuiteRelative", TestSuite);
        }

        protected void AddToDico(string key, string value)
        {
            if (Dico.Keys.Contains(key))
                throw new ArgumentException("This key already exists.");

            Dico.Add(key, value);
        }

        protected virtual void OnPreCalculateDico()
        {
            
        }

        protected virtual void OnPostCalculateDico()
        {
            
        }

        protected void BuildConfigFile()
        {
            var templateText = GetTemplateText(ConfigTemplateFilename);
            var content = Apply(templateText, Dico);
            filePersister.Save(ConfigFullPath, content);
        }
        protected void BuildRunnerProjectFile()
        {
            var templateText = GetTemplateText(RunnerProjectTemplateFilename);
            var content = Apply(templateText, Dico);
            filePersister.Save(RunnerProjectFullPath, content);
        }

        protected virtual void CopyRuntimeFile()
        {

        }

        protected string Apply(string templateText, Dictionary<string, string> keyValues)
        {
            Template template = new Template(templateText, '$', '$');
            foreach (var key in keyValues.Keys)
                template.Add(key, keyValues[key]);

            var str = template.Render();

            return str;
        }

        protected string GetTemplateText(string filename)
        {
            string templateText = string.Empty;
            
            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Service.RunnerConfig.Resources." + filename))
            using (StreamReader reader = new StreamReader(stream))
            {
                templateText = reader.ReadToEnd();
            }
            return templateText;
        }

       
    }
}
