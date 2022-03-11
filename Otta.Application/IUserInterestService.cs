using Otta.Application.Dto;

namespace Otta.Application
{
    public interface IUserInterestService
    {

        UserSimilartyScoreDto GetUserJobsSimilartyScore(bool direction);

        CompanySimilartyScoreDto GetCompanySimilartyScore(bool direction);
    }
}