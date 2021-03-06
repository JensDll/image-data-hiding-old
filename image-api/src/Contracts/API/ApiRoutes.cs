using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.API
{
    public static class ApiRoutes
    {
        private const string Base = "api";

        public static class ImageRoutes
        {
            public const string EncodeRandom = Base + "/image/encode/random";

            public const string EncodeFile = Base + "/image/encode/file";

            public const string Decode = Base + "/image/decode";
        }

        public static class UserRoutes
        {
            public const string GetAll = Base + "/user";

            public const string GetById = Base + "/user/{id}";

            public const string GetByUsername = Base + "/user/name/{username}";

            public const string IsUserNameTake = Base + "/user/name/{username}/taken";
        }
    }
}
