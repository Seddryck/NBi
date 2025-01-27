using System.Data;
using System.IO;
using Moq;
using NBi.Core;
using NBi.Core.Query;
using NBi.Core.ResultSet;
using NBi.Testing;
using NUnit.Framework;

namespace NBi.Core.Testing;

[TestFixture]
public class ResultSetManagerTest
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
        if (Directory.Exists(FileOnDisk.GetDirectoryPath() + @"\Queries"))
            Directory.Delete(FileOnDisk.GetDirectoryPath() + @"\Queries", true);
        if (Directory.Exists(FileOnDisk.GetDirectoryPath() + @"\Expect"))
            Directory.Delete(FileOnDisk.GetDirectoryPath() + @"\Expect", true);
    }
    #endregion


    //[Test]
    //public void CreateResultSet_ForTwoQueries_ExecuteTwoQueriesAndCreateTwoResultSets()
    //{
    //    //setup environment
    //    var path = FileOnDisk.GetDirectoryPath() + @"\Queries";
    //    if (Directory.Exists(path))
    //        Directory.Delete(path,true);
    //    Directory.CreateDirectory(path);

    //    File.Create(path + @"\Query_1.mdx").Close();
    //    File.Create(path + @"\Query_2.mdx").Close();
        
    //    //Setup Mocks and Object to test
    //    var mockResultSetWriter = new Mock<IResultSetWriter>();
    //    mockResultSetWriter.SetupProperty(rsw => rsw.PersistencePath, FileOnDisk.GetDirectoryPath() + @"\Expect");
    //    IResultSetWriter resultSetWriter = mockResultSetWriter.Object;
        
    //    var mockQueryExecutor = new Mock<IQueryExecutor>();
    //    mockQueryExecutor.Setup(qe => qe.Execute()).Returns(new DataSet());
    //    IQueryExecutor queryExecutor = mockQueryExecutor.Object;

    //    var rsm = new ResultSetManager(
    //        resultSetWriter
    //        , queryExecutor);           

    //    //Method under test
    //    rsm.CreateResultSet(path);

    //    //Test conclusion            
    //    mockQueryExecutor.Verify(qe => qe.Execute(), Times.Exactly(2));
    //    mockResultSetWriter.Verify(rsw => rsw.Write("Query_1.csv", It.IsAny<DataSet>()), Times.Once());
    //    mockResultSetWriter.Verify(rsw => rsw.Write("Query_2.csv", It.IsAny<DataSet>()), Times.Once());
        
    //}
}
