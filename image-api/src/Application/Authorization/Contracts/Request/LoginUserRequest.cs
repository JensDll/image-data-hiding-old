﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    public record LoginUserRequest(string Username, string Password);
}
