using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizinatorCore.Services;
using QuizinatorCore.Interfaces;
using QuizinatorCore.Entities;
using QuizinatorCore.Entities.Idioms;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuizinatorInfrastructure.Services; // TODO: remove dependency!

namespace QuizinatorUI
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
            services.AddControllersWithViews();
            services.AddTransient<IDatabaseService<Idiom>, IdiomsJsonFileService>();
            services.AddTransient<IDatabaseService<Quiz>, QuizzesJsonFileService>();
            services.AddTransient<FileConverter>();
            services.AddTransient<ISorter<Idiom>, IdiomSorter>();
            services.AddTransient<ISorter<Quiz>, QuizSorter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
