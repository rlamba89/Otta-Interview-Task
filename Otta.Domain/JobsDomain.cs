using System.Collections.Generic;
using System.Linq;

namespace Otta.Domain
{
    public class JobsDomain
    {
        public JobsDomain(List<UserCompanyDomain> userCompanies)
        {
            _userCompanies = userCompanies;
            CalculateSimilartyScore();
        }

        private List<UserCompanyDomain> _userCompanies;
        public IEnumerable<UserCompanyDomain> UserCompanies => _userCompanies;

        public int SimilartyScore { get; private set; }

        public int CompanyId1 { get; private set; }

        public int CompanyId2 { get; private set; }

        private void CalculateSimilartyScore()
        {
            if (_userCompanies.Count == 0) return;

            var userAppliedCompanies = _userCompanies.Select(a => new { a.UserId, a.CompanyIds }).ToList();

            var flattenedCompanyIds = userAppliedCompanies.SelectMany(a => a.CompanyIds).Distinct().ToList();

            var companyPairs = from company1 in flattenedCompanyIds
                               from company2 in flattenedCompanyIds
                               where company1.CompareTo(company2) < 0
                               select new { company1, company2 };

            var result = (from company in userAppliedCompanies
                          from pair in companyPairs
                          where company.CompanyIds.Contains(pair.company1) && company.CompanyIds.Contains(pair.company2)
                          group company by pair into g
                          orderby g.Count() descending
                          select g);

            if (!result.Any()) return;

            var score = result.First();

            SimilartyScore = score.Count();

            CompanyId1 = score.Key.company1;

            CompanyId2 = score.Key.company2;
        }

    }

    public class UserCompanyDomain
    {
        public int UserId { get; private set; }

        public UserCompanyDomain(int userId)
        {
            UserId = userId;
        }

        public IEnumerable<int> CompanyIds { get; set; }
    }
}
