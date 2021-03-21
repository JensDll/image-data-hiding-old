using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization.Domain
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime RegistrationDate { get; set; }

        public DateTime DeletionDate { get; set; }
    }
}
