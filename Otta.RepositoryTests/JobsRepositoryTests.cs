using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Otta.Repository;
using Otta.Utility;
using System;
using System.Data;
using System.Linq;

namespace Otta.RepositoryTests
{
    [TestClass]
    public class JobsRepositoryTests
    {
        private Mock<IReadCsv> _readCsv;
        private JobsRepository _jobsRepository;

        [TestInitialize]
        public void Initialize()
        {
            _readCsv = new Mock<IReadCsv>();

            _jobsRepository = new JobsRepository(_readCsv.Object);
        }

        [DataRow(true, 3)]
        //[DataRow(false, 1)]
        [DataTestMethod]
        public void GetJobs_ShouldFetchData_FromCSV(bool direction, int expectedRows)
        {
            var jobsData = GetJobsData();
            var reactionsData = GetReactionsData();

            _readCsv.Setup(a => a.GetFileData(It.Is<string>(s => s.Contains("jobs.csv")))).Returns(jobsData);
            _readCsv.Setup(a => a.GetFileData(It.Is<string>(s => s.Contains("reactions.csv")))).Returns(reactionsData);

            var jobs = _jobsRepository.GetJobs(direction);

            Assert.IsNotNull(jobs);
            Assert.IsTrue(jobs.UserCompanies.Count() == expectedRows);
        }

        private DataTable GetJobsData()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("company_id");
            dt.Columns.Add("job_id");
            AddRows(dt, 1, 100);
            AddRows(dt, 2, 200);
            AddRows(dt, 3, 300);
            AddRows(dt, 4, 400);

            return dt;
        }

        private DataTable GetReactionsData()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("direction");
            dt.Columns.Add("user_id");
            dt.Columns.Add("job_id");
            dt.Columns.Add("time");

            AddRows(dt, 1, 100, true, DateTime.Now.AddDays(-10));
            AddRows(dt, 2, 200, true, DateTime.Now.AddDays(10));
            AddRows(dt, 3, 300, false, DateTime.Now.AddDays(-1));
            AddRows(dt, 4, 400, true, DateTime.Now.AddDays(-9));

            return dt;
        }

        private static void AddRows(DataTable dt, int userId, int jobId, bool direction, DateTime time)
        {
            DataRow rows = dt.NewRow();
            rows["user_id"] = userId.ToString();
            rows["job_id"] = jobId.ToString();
            rows["direction"] = direction.ToString();
            rows["time"] = time.ToString();

            dt.Rows.Add(rows);
        }

        private static void AddRows(DataTable dt, int companyId, int jobId)
        {
            DataRow rows = dt.NewRow();
            rows["company_id"] = companyId;
            rows["job_id"] = jobId;
            dt.Rows.Add(rows);
        }
    }
}
