using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class ApiRoutes
    {
        public const string Base = "api";

        public static class ImageRoutes
        {
            public const string Encode = Base + "/image/encode";

            public const string Decode = Base + "/image/decode";

            public const string Test = Base + "/image/test";
        }
    }

    public static class IdentityRoutes
    {
        public const string Base = "identiy";

        public static class AccountRoutes
        {
            public const string Register = Base + "/account/register";

            public const string Login = Base + "/account/login";
        }
    }
}
