using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using musicMash.Models;
using musicMash.Repositories;
using musicMash.Services;

namespace musicMash
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddMvcOptions(o => o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()));
            services.AddScoped<IMashupService, MashupService>();
            services.AddScoped<IRepository<CoverArtResult>, Repository<CoverArtResult>>();
            services.AddScoped<IRepository<MusicBrainzResult>, Repository<MusicBrainzResult>>();
            services.AddScoped<IRepository<WikipediaResult>, Repository<WikipediaResult>>();
            services.AddScoped<IHttpHandler, HttpClientHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }
            app.UseStatusCodePages();
            app.UseMvc();
        }
    }
}
