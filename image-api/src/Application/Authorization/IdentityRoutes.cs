using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    public static class IdentityRoutes
    {
        public const string Base = "identity";

        public static class AccountRoutes
        {
            public const string Register = Base + "/account/register";

            public const string Login = Base + "/account/login";

            public const string Logout = Base + "/account/logout";

            public const string Refresh = Base + "/account/refresh";

            public const string Delete = Base + "/account/delete";
        }
    }
}
