using System;

namespace Contracts.API.Response
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public long TimeUntilDeletion { get; set; }
    }
}
