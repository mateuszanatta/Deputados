using Congressperson.Controllers;
using Congressperson.Controllers.Interfaces;
using Congressperson.HttpClients;
using Congressperson.HttpClients.Interfaces;
using Congressperson.Models;
using Congressperson.Services;
using Congressperson.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Congressperson
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

            services.Configure<CongresspersonDatabaseSettings>(
                Configuration.GetSection(nameof(CongresspersonDatabaseSettings)));
            services.AddSingleton<ICongresspersonDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CongresspersonDatabaseSettings>>().Value);

            services.AddSingleton<ICongresspersonService, CongresspersonService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Deputados", Version = "v1" });
            });
            services.AddScoped<IHttpClients, DefaultHttpClient>();
            services.AddScoped<ICongresspersonAPICall, CongresspersonAPICall>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Deputados v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
