#region Using directives
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Assemblies;
using NUnit.Framework;

#endregion
namespace NBi.Testing.Unit.Core.Assemblies
{
    [TestFixture]
    public class AssemblyManagerTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void GetInstance_ExistingTypeConstructoreWithZeroParam_InstantiatedAndNotNull()
        {           
            //Build the SUT
            var am = new AssemblyManager();

            //Call the method to test
            var actual = am.GetInstance(DiskOnFile.GetDirectoryPath() + "NBi.Testing.dll", "NBi.Testing.Unit.Core.Assemblies.Resource.Klass", null);

            //Assertion
            Assert.IsInstanceOf<NBi.Testing.Unit.Core.Assemblies.Resource.Klass>(actual);
            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public void Execute_ExistingTypeConstructoreWithZeroParam_PublicMethodOneParameter()
        {
            //Build the SUT
            var am = new AssemblyManager();
            var klass = new NBi.Testing.Unit.Core.Assemblies.Resource.Klass();
            var paramDico = new Dictionary<string, object>();
            paramDico.Add("paramString", "MyString");

            //Call the method to test
            var actual = am.Execute(klass, "ExecutePublicString", paramDico);

            //Assertion
            Assert.That(actual, Is.EqualTo("Executed"));
        }

        [Test]
        public void Execute_ExistingTypeConstructoreWithZeroParam_PrivateMethodOneParameter()
        {
            //Build the SUT
            var am = new AssemblyManager();
            var klass = new NBi.Testing.Unit.Core.Assemblies.Resource.Klass();
            var paramDico = new Dictionary<string, object>();
            paramDico.Add("paramString", "MyString");

            //Call the method to test
            var actual = am.Execute(klass, "ExecutePrivateString", paramDico);

            //Assertion
            Assert.That(actual, Is.EqualTo("Executed"));
        }


        [Test]
        public void Execute_ExistingTypeConstructoreWithZeroParam_SeveralParameter()
        {
            //Build the SUT
            var am = new AssemblyManager();
            var klass = new NBi.Testing.Unit.Core.Assemblies.Resource.Klass();
            var paramDico = new Dictionary<string, object>();
            
            //Reverse param order to ensure they are correctly re-ordered!
            paramDico.Add("paramEnum", "Beta");
            paramDico.Add("paramDateTime", "2012-05-07");
            
            //Call the method to test
            var actual = am.Execute(klass, "ExecuteDoubleParam", paramDico);

            //Assertion
            Assert.That(actual, Is.EqualTo("Executed"));
        }

        [Test]
        public void Execute_ExistingType_GetTypeForStaticUsage()
        {
            //Build the SUT
            var am = new AssemblyManager();
            
            //Call the method to test
            var actual = am.GetStatic(DiskOnFile.GetDirectoryPath() + "NBi.Testing.dll", "NBi.Testing.Unit.Core.Assemblies.Resource.StaticKlass");

            //Assertion
            Assert.That(actual.FullName,Is.EqualTo("NBi.Testing.Unit.Core.Assemblies.Resource.StaticKlass"));
        }

        [Test]
        public void Execute_ExistingType_StaticMethod()
        {
            //Build the SUT
            var am = new AssemblyManager();
            var paramDico = new Dictionary<string, object>();

            //Reverse param order to ensure they are correctly re-ordered!
            paramDico.Add("paramString", "MyString");
            
            //Call the method to test
            var actual = am.ExecuteStatic(typeof(NBi.Testing.Unit.Core.Assemblies.Resource.StaticKlass), "ExecuteStaticString", paramDico);

            //Assertion
            Assert.That(actual, Is.EqualTo("Executed"));
        }
    }
}
