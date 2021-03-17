using Application.Common.Interfaces.Services;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEnumerableService, EnumerableService>();
            services.AddTransient<IEncodeService, EncodeService>();
            services.AddTransient<IDecodeService, DecodeService>();
        }
    }
}
