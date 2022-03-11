using Otta.Domain;
using Otta.Domain.Repository;
using Otta.Utility;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Otta.Repository
{
    public class ReactionRepository : IReactionsRepository
    {
        private const string JobsFileName = "reactions.csv";
        private const string FolderName = "Data";
        public readonly IReadCsv _readCsv;

        public ReactionRepository(IReadCsv readCsv)
        {
            _readCsv = readCsv;
        }

        public ReactionsDomain GetReactions(bool direction)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), FolderName, JobsFileName);

            var fileData = _readCsv.GetFileData(path);

            var userJobs = fileData.AsEnumerable().Where(row => Convert.ToBoolean(row.Field<string>("direction")) == direction)
                                                  .Select(row => new
                                                  {
                                                      jobId = Convert.ToInt32(row.Field<string>("job_id")),
                                                      userId = Convert.ToInt32(row.Field<string>("user_id"))

                                                  }).ToList();
           var appliedJobs = userJobs.GroupBy(a => new { a.jobId }).Select(u => new UserJobsDomain(u.Key.jobId)
            {
                Users = u.GroupBy(x => new { x.userId }).Select(y => y.Key.userId).Distinct().ToList()
            });

            return new ReactionsDomain(appliedJobs.ToList());
        }
    }
}
