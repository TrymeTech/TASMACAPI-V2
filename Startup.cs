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
           options.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://180.179.49.72:8083", "http://180.179.49.72:80", "http://180.179.49.72", "http://hms.testandverification.com/ems-app", "http://hms.testandverification.com/hms-app", "http://tncsc-scm.in", "http://localhost:443", "https://tncsc-scm.in:443", "https://www.tncsc-scm.in", "http://180.179.49.72:8085", "http://localhost:4100", "http://180.179.49.72:8081", "http://hms.cctv.tasmac.co.in/hms-app", "http://hms.cctv.tasmac.co.in/ems-app", "https://hms.cctv.tasmac.co.in/hms-app", "https://hms.cctv.tasmac.co.in/ems-app", "http://hms.cctv.tasmac.co.in", "https://hms.cctv.tasmac.co.in")
           .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowCredentials()
         );
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
