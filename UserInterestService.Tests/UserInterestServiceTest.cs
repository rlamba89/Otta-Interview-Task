using Microsoft.VisualStudio.TestTools.UnitTesting;
using Otta.Application;
using Otta.Domain.Repository;
using Otta.Repository;
using Otta.Utility;

namespace Otta.ApplicationServiceTests
{
    [TestClass]
    public class UserInterestServiceTest
    {
        private IReactionsRepository _reactionsRepository;
        private UserInterestService _userInterestService;
        private IJobsRepository _jobsRepository;

        [TestInitialize]
        public void Initialize()
        {
            _reactionsRepository = new ReactionRepository(new ReadCsv());
            _jobsRepository = new JobsRepository(new ReadCsv());


            _userInterestService = new UserInterestService(_reactionsRepository, _jobsRepository);
        }

        [TestMethod]
        public void GetReactionsScore_ShouldReturn_TopTwoUsersSimilartyScore()
        {
            var score = _userInterestService.GetUserJobsSimilartyScore(true);
            Assert.IsNotNull(score);
            Assert.IsTrue(score.SimilartyScore == 181);
            Assert.IsTrue(score.UserId1 == 1791);
            Assert.IsTrue(score.UserId2 == 5193);
        }

        [TestMethod]
        public void GetCompaniesScore_ShouldReturn_TopTwoCompaniesSimilartyScore()
        {
            var score = _userInterestService.GetCompanySimilartyScore(true);

            Assert.IsNotNull(score);
            
            Assert.IsTrue(score.SimilartyScore == 104);
            Assert.IsTrue(score.CompanyId1 == 46);
            Assert.IsTrue(score.CompanyId2 == 92);
        }
    }
}
