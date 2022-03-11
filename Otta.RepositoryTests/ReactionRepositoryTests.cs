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
    public class ReactionRepositoryTests
    {
        private Mock<IReadCsv> _readCsv;
        private ReactionRepository _reactionRepository;

        [TestInitialize]
        public void Initialize()
        {
            _readCsv = new Mock<IReadCsv>();

            _reactionRepository = new ReactionRepository(_readCsv.Object);
        }

        [DataRow(true, 3)]
        [DataRow(false, 1)]
        [DataTestMethod]
        public void GetReaction_ShouldFetchData_FromCSV(bool direction, int expectedRows)
        {
            var jobsData = GetJobsData();
            _readCsv.Setup(a => a.GetFileData(It.IsAny<string>())).Returns(jobsData);

            var jobs = _reactionRepository.GetReactions(direction);

            Assert.IsNotNull(jobs);
            Assert.IsTrue(jobs.UserJobs.Count() == expectedRows);
        }

        private DataTable GetJobsData()
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
    }
}
