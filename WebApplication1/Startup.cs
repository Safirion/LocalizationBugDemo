using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApplication1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddControllersWithViews()
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization((options => {
                    options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(SharedResource));
                }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string[] supportedCultures = { "en-US", "fr-FR" };
            app.UseRequestLocalization(options =>
                options.AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures)
                    .SetDefaultCulture("fr-FR")
                    .RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context => Task.FromResult(new ProviderCultureResult("fr-FR"))))
            );

            app.UseRouting();

#if DEBUG
            app.UseDeveloperExceptionPage();
#endif

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }
}
