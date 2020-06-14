using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaymentGateway.Models;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway{
    public class Startup{
        public IConfiguration Configuration {get;}

        public Startup(IConfiguration configuration) => Configuration = configuration;
        public void ConfigureServices(IServiceCollection services){
            services.AddDbContext<PaymentContext>(opt => opt.UseSqlServer(Configuration["Data:PaymentGatewayAPIConnection:ConnectionString"]));
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
            app.UseMvc();
        }
    }
}
