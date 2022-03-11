using System.Collections.Generic;

namespace Otta.Domain.Repository
{
    public interface IJobsRepository
    {
        JobsDomain GetJobs(bool direction);
    }
}
