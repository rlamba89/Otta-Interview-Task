using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Otta.Application;
using Otta.Application.Dto;

namespace Otta.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimilartyScoreController : ControllerBase
    {
        private readonly ILogger<SimilartyScoreController> _logger;
        private readonly IUserInterestService _userInterestService;

        public SimilartyScoreController(ILogger<SimilartyScoreController> logger, IUserInterestService userInterestService)
        {
            _logger = logger;
            _userInterestService = userInterestService;
        }

        [HttpGet]
        [Route("users/{direction}")]
        public UserSimilartyScoreDto GetUserSimilartyScore(bool direction)
        {
            return _userInterestService.GetUserJobsSimilartyScore(direction);
        }


        [HttpGet]
        [Route("companies/{direction}")]
        public CompanySimilartyScoreDto GetCompanySimilartyScore(bool direction)
        {
            return _userInterestService.GetCompanySimilartyScore(direction);
        }
    }
}
