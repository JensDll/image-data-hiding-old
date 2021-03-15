﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public static class ApiRoutes
    {
        public const string Base = "api";

        public static class ImageRoutes
        {
            public const string EncodeRandom = Base + "/image/encode/random";

            public const string EncodeFile = Base + "/image/encode/file";

            public const string Decode = Base + "/image/decode";
        }
    }
}
