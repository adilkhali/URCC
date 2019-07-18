﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;
using UnitedRemote.Core;
using UnitedRemote.Core.Helpers;
using UnitedRemote.Core.Helpers.ErrorHandler;
using UnitedRemote.Core.Models.V1;
using UnitedRemote.Core.Repositories;
using UnitedRemote.Core.Repositories.Interfaces;

namespace United_Remote_Coding_Challeng.Infrastructure
{
    internal static class Installer
    {

        public static void ConfigureDataBase(IServiceCollection services)
        {
            // Add DbContext 
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("URcc"));
            // Add Identity
            services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();




            // IOC
            services.AddTransient<IErrorHandler, ErrorHandler>();
            services.AddTransient<IUsersRepository, UsersRepository>();
        }

        public static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "United Remote CC BackEnd API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                });
            });

            // Add Api Versioning
            services.AddApiVersioning();
        }

        public static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["JwtIssuer"],
                        ValidAudience = configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

    }
}