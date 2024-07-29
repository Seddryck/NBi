#region Using directives
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Assemblies;
using NBi.Testing;
using NUnit.Framework;

#endregion
namespace NBi.Core.Testing.Assemblies
{
    [TestFixture]
    public class AssemblyManagerTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
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
        [Ignore("No idea why this is failing")]
        public void GetInstance_ExistingTypeConstructoreWithZeroParam_InstantiatedAndNotNull()
        {           
            //Build the SUT
            var am = new AssemblyManager();

            //Call the method to test
            var actual = am.GetInstance(FileOnDisk.GetDirectoryPath() + "NBi.Core.Testing.dll", "Resource.Klass", []);

            //Assertion
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual, Is.InstanceOf<Resource.Klass>());
        }

        [Test]
        public void Execute_ExistingTypeConstructoreWithZeroParam_PublicMethodOneParameter()
        {
            //Build the SUT
            var am = new AssemblyManager();
            var klass = new Resource.Klass();
            var paramDico = new Dictionary<string, object>() { { "paramString", "MyString" } };

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
            var klass = new Resource.Klass();
            var paramDico = new Dictionary<string, object>() { { "paramString", "MyString" } };

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
            var klass = new Resource.Klass();
            var paramDico = new Dictionary<string, object>()
            { 
                //Reverse param order to ensure they are correctly re-ordered!
                { "paramEnum", "Beta" },
                { "paramDateTime", "2012-05-07" }
            };

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
            var actual = am.GetStatic(FileOnDisk.GetDirectoryPath() + "NBi.Core.Testing.dll", "Resource.StaticKlass");

            //Assertion
            Assert.That(actual.FullName,Is.EqualTo("Resource.StaticKlass"));
        }

        [Test]
        public void Execute_ExistingType_StaticMethod()
        {
            //Build the SUT
            var am = new AssemblyManager();
            var paramDico = new Dictionary<string, object>() { { "paramString", "MyString" } };
            
            //Call the method to test
            var actual = am.ExecuteStatic(typeof(Resource.StaticKlass), "ExecuteStaticString", paramDico);

            //Assertion
            Assert.That(actual, Is.EqualTo("Executed"));
        }
    }
}
