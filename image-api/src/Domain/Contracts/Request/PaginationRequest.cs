﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Request
{
    public record PaginationRequest
    {
        private int pageNumber;
        private int pageSize;

        public PaginationRequest()
        {
            pageNumber = 1;
            pageSize = 10;
        }

        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = value > 0 ? value : 1;
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > 200 ? 200 : value;
        }
    }
}
