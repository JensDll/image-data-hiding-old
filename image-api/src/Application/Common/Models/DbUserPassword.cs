using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class DbUserPassword
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime DeletionDate { get; set; }
    }
}
