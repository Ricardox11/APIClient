using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using AppClient.Controllers;
using System.Text.Json.Serialization;

namespace AppClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            // usar consulta de iconfiguration sin instancia
            ConfigurationApp.Configuration = configuration;



        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            // configuracion swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "client", Version = "v1" });
            });

            // servicio de conexion a BD
            // services.AddDbContext<ClientContext>(options =>
            //       options.UseSqlite(Configuration.GetConnectionString("ClientContext")));

            // adicionar contexto Azure - Sqlserver - relacion con conexion 
            services.AddDbContext<ClientContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ClientContext")));

            // usar instancia de iconfiguration
            //services.AddTransient<ConfigurationApp>();


            // SP - DI para llamar e instanciar
            services.AddScoped<ClientRepository>();

            // evitar referencia circular entre relaciones de navegacion virtual
           services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // configuracion swagger
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "client v1"));
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
