﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Response
{
    public class Envelop<T>
    {
        public T Data { get; set; }
    }
}
