using CompraAplicativos.Application.UseCases.Aplicativo.ObterAplicativos;
using CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente;
using CompraAplicativos.Core.Aplicativos;
using CompraAplicativos.Core.Clientes;
using CompraAplicativos.Infrastructure.DataAccess.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace CompraAplicativos.Api
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
            services.AddScoped<IObterAplicativosUseCase, ObterAplicativosUseCase>();
            services.AddScoped<IAplicativoRepository, AplicativoRepository>();

            services.AddScoped<ICadastrarClienteUseCase, CadastrarClienteUseCase>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            services.AddMemoryCache();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new QueryStringApiVersionReader();
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
            });
            services.AddVersionedApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
            });

            services.AddControllers();

            IApiVersionDescriptionProvider provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(config =>
            {
                config.EnableAnnotations();

                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    config.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = "api-compra-aplicativo",
                        Version = description.ApiVersion.ToString()
                    });
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(config =>
            {
                config.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    httpReq.IsHttps = true;
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                });
            });

            app.UseSwaggerUI(config =>
            {
                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    config.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                config.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
