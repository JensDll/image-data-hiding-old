using Application.API.Interfaces;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(null);
        }
    }
}
