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
    public class JobsRepository : IJobsRepository
    {
        private const string JobsFileName = "jobs.csv";
        private const string ReactionsFileName = "reactions.csv";

        private readonly IReadCsv _readCsv;

        public JobsRepository(IReadCsv readCsv)
        {
            _readCsv = readCsv;
        }

        public JobsDomain GetJobs(bool direction)
        {
            var jobsFileData = GetFileData(JobsFileName);
            var reactionsFileData = GetFileData(ReactionsFileName);

            var appliedJobs = jobsFileData.AsEnumerable().Select(row => new 
            {
                CompanyId = Convert.ToInt32(row.Field<string>("company_id")),
                JobId = Convert.ToInt32(row.Field<string>("job_id"))
            }).ToList();

            var userReactions = reactionsFileData.AsEnumerable().Where(row => Convert.ToBoolean(row.Field<string>("direction")) == direction)
                                                                .Select(row => new
                                                                {
                                                                    UserId = Convert.ToInt32(row.Field<string>("user_id")),
                                                                    JobId = Convert.ToInt32(row.Field<string>("job_id"))
                                                                }).ToList();

            var joinedData = from reactions in userReactions
                          from jobs in appliedJobs
                          where reactions.JobId == jobs.JobId
                          select new { jobs.JobId, reactions.UserId, jobs.CompanyId };


            var users = joinedData.GroupBy(a => new { a.UserId }).Select(u => new UserCompanyDomain(u.Key.UserId)
            {
                CompanyIds = u.GroupBy(x => new { x.CompanyId }).Select(y => y.Key.CompanyId).Distinct().ToList()
            });

            return new JobsDomain(users.ToList());
        }

        private DataTable GetFileData(string fileName)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", fileName);

            var fileData = _readCsv.GetFileData(path);

            return fileData;
        }
    }
}
