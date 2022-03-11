using System.Collections.Generic;
using System.Linq;

namespace Otta.Domain
{
    public class ReactionsDomain
    {
        public ReactionsDomain(List<UserJobsDomain> userJobs)
        {
            _userJobs = userJobs;
            CalcualteSimilartyScore();
        }

        private List<UserJobsDomain> _userJobs;
        public IEnumerable<UserJobsDomain> UserJobs => _userJobs;

        public int SimilartyScore { get; private set; }

        public int UserId1 { get; private set; }

        public int UserId2 { get; private set; }

        private void CalcualteSimilartyScore()
        {
            var userAppliedJobs = _userJobs.Select(a => new { a.Users, a.JobId }).ToArray();

            var flattenedUserIds = userAppliedJobs.SelectMany(a => a.Users).Distinct();

            var userPairs = from user1 in flattenedUserIds
                            from user2 in flattenedUserIds
                            where user1.CompareTo(user2) < 0
                            select new { user1, user2 };

            var result = (from user in userAppliedJobs
                          from pair in userPairs
                          where user.Users.Contains(pair.user1) && user.Users.Contains(pair.user2)
                          group user by pair into g
                          orderby g.Count() descending
                          select g).ToList();

            if (!result.Any()) return;

            var score = result.First();

            SimilartyScore = score.Count();

            UserId1 = score.Key.user1;

            UserId2 = score.Key.user2;
        }
    }

    public class UserJobsDomain
    {
        public int JobId { get; private set; }

        public UserJobsDomain(int jobId)
        {
            JobId = jobId;
        }

        public IEnumerable<int> Users { get; set; }
    }
}
