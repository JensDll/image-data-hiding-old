using Application.Common.Interfaces;
using Domain.Contracts.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public abstract class RepositoryBase
    {
        public IConnectionFactory ConnectionFactory { get; }

        protected RepositoryBase(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        protected static (int Skip, int Take) GetSkipTake(PaginationRequest request)
        {
            int skip = request.PageNumber * request.PageSize - request.PageSize;
            int take = request.PageSize;

            return (skip, take);
        }
    }
}
