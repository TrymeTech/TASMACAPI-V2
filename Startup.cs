using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(options =>
           options.WithOrigins("http://localhost:4200", "http://localhost:81", "http://172.16.80.21:84", "http://172.16.80.15:81", "https://hms.cctv.tasmac.co.in/hms-app", 
           "http://hms.cctv.tasmac.co.in/hms-app", "https://hms.cctv.tasmac.co.in", "http://hms.cctv.tasmac.co.in", "https://hms.cctv.tasmac.co.in/ems-app", "http://hms.cctv.tasmac.co.in/ems-app")
           .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials()
         );
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
