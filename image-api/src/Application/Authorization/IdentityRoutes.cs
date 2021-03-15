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
        }
    }
}
