using Application.API.Interfaces;
using Application.Data;
using Contracts.API.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.API.Repositories
{
    internal abstract class RepositoryBase
    {
        public IConnectionFactory ConnectionFactory { get; }

        protected RepositoryBase(IConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        protected static (int Skip, int Take) GetSkipTake(PaginationRequestDto request)
        {
            int skip = request.PageNumber * request.PageSize - request.PageSize;
            int take = request.PageSize;

            return (skip, take);
        }
    }
}
