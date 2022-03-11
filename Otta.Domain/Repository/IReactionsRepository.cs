using System.Collections.Generic;

namespace Otta.Domain.Repository
{
    public interface IReactionsRepository
    {
        ReactionsDomain GetReactions(bool direction);
    }
}
