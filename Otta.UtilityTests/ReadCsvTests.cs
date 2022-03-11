using Microsoft.VisualStudio.TestTools.UnitTesting;
using Otta.Utility;
using System.IO;
using System.Reflection;

namespace Otta.UtilityTests
{
    [TestClass]
    public class ReadCsvTests
    {
        private ReadCsv _readCsv;

        [TestInitialize]
        public void Initialize()
        {
            _readCsv = new ReadCsv();
        }

        [DataRow("jobs.csv")]
        [DataRow("reactions.csv")]
        [DataTestMethod]
        public void ForAGivenPath_GetFileData_ShouldReturnCsvData(string fileName)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", fileName);

            var data = _readCsv.GetFileData(path);

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Rows.Count > 0);
        }
    }
}
