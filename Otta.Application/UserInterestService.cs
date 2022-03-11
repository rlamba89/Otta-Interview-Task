using Otta.Application.Dto;
using Otta.Domain.Repository;

namespace Otta.Application
{
    public class UserInterestService : IUserInterestService
    {
        private readonly IReactionsRepository _reactionsRepository;
        private readonly IJobsRepository _jobsRepository;

        public UserInterestService(IReactionsRepository reactionsRepository, IJobsRepository jobsRepository)
        {
            _reactionsRepository = reactionsRepository;
            _jobsRepository = jobsRepository;
        }

        public UserSimilartyScoreDto GetUserJobsSimilartyScore(bool direction)
        {
            var userJobReactions = _reactionsRepository.GetReactions(direction);

            return new UserSimilartyScoreDto
            {
                SimilartyScore = userJobReactions.SimilartyScore,
                UserId1 = userJobReactions.UserId1,
                UserId2 = userJobReactions.UserId2
            };
        }

        public CompanySimilartyScoreDto GetCompanySimilartyScore(bool direction)
        {
            var userReactions = _jobsRepository.GetJobs(direction);

            return new CompanySimilartyScoreDto
            {
                SimilartyScore = userReactions.SimilartyScore,
                CompanyId1 = userReactions.CompanyId1,
                CompanyId2 = userReactions.CompanyId2
            };
        }
    }
}

