﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IConnectionFactory
    {
        IDbConnection NewConnection { get; }
    }
}
