using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using United_Remote_Coding_Challeng.Configuration;
using United_Remote_Coding_Challeng.Infrastructure;
using United_Remote_Coding_Challeng.Infrastructure.Middlewares;


namespace United_Remote_Coding_Challeng
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Enable Cros
            services.AddCors();

            Installer.ConfigureDataBase(services);
            // Add Jwt Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Installer.ConfigureAuthentication(services, Configuration);

            // Add Swagger
            Installer.ConfigureSwagger(services);

            services.AddMvc()
                .AddJsonOptions(options => options.UseMemberCasing())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseStatusCodePages();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            // Swagger configuration
            var swaggerSettings = new SwaggerSettings();
            Configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerSettings);
            app.UseSwagger(option => {option.RouteTemplate = swaggerSettings.JsonRoute;});
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerSettings.UIEndpoint, swaggerSettings.Description); });


            app.UseMvc();
        }
    }
}
