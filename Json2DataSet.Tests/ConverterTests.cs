using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Json2DataSet.Tests.Properties;

namespace Json2DataSet.Tests
{
    [TestClass]
    public class ConverterTests
    {
        [TestMethod]
        public void DefaultBaseTableNameTest()
        {
            var dataSet = Json2DataSetConverter.Convert(Helper.ReadJsonFromFile("test001.json"));
            Assert.AreEqual(1, dataSet.Tables.Count);
            Assert.AreEqual("#", dataSet.Tables[0].TableName);
        }

        [TestMethod]
        public void BaseTableNameTest()
        {
            var dataSet = Json2DataSetConverter.Convert(Helper.ReadJsonFromFile("test001.json"),
                new ConvertOptions { BaseTableName = "Person" });
            Assert.AreEqual(1, dataSet.Tables.Count);
            Assert.AreEqual("Person", dataSet.Tables[0].TableName);
        }
    }
}
