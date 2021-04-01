using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Authorization
{
    public static class AuthRoutes
    {
        private const string Base = "auth";

        public static class AccountRoutes
        {
            public const string Register = Base + "/register";

            public const string Login = Base + "/login";

            public const string Logout = Base + "/logout";

            public const string Refresh = Base + "/refresh";

            public const string Delete = Base + "/delete";
        }
    }
}
