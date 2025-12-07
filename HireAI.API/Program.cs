using HireAI.API.Extensions;
using HireAI.Data.Models.Identity;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.Mappings;
using HireAI.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using HireAI.Service.Interfaces;
using HireAI.Infrastructure.GenericBase;
using HireAI.API.Extensions;
using Amazon.S3;
using Amazon.Extensions.NETCore.Setup;
using Amazon;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;




namespace HireAI.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            // Register DbContext
            builder.Services.AddDbContext<HireAIDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("HireAiDB"));
            });

            #region a

            // Register Identity
            builder.Services.AddApplicationIdentity();
            //REGISTER ApplicationUser and IdentityRole with the DI Container
    


            // Register repositories and services using extension methods
            builder.Services.AddApplicationRepositories();
            builder.Services.AddApplicationServices();
            #endregion

            // Add AutoMapper service
            builder.Services.AddAutoMapper(cfg => { }, typeof(ApplicationProfile).Assembly);


            // Configure AWS S3 with credentials from appsettings
            var awsAccessKey = builder.Configuration["AWS:AccessKey"];
            var awsSecretKey = builder.Configuration["AWS:SecretKey"];
            var awsRegion = builder.Configuration["AWS:Region"];

            // Replace the incorrect AddAWSService registration with the correct usage of AWSOptions
            var awsOptions = builder.Configuration.GetAWSOptions();
            awsOptions.Credentials = new Amazon.Runtime.BasicAWSCredentials(awsAccessKey, awsSecretKey);
            awsOptions.Region = RegionEndpoint.GetBySystemName(awsRegion);
            builder.Services.AddDefaultAWSOptions(awsOptions);
            builder.Services.AddAWSService<IAmazonS3>();

            #region Configure CORS to allow requests from any origin
            builder.Services.AddCors(options =>
            {
                // Policy 1: Allow everything (for development)
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });

                // Policy 2: Restrict to specific origin (for production)
                options.AddPolicy("ProductionPolicy", builder =>
                {
                    builder.WithOrigins("https://myapp.com", "https://www.myapp.com")
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });

                // Policy 3: Read-only access
                options.AddPolicy("ReadOnly", builder =>
                {
                    builder.AllowAnyOrigin()
                           .WithMethods("GET")
                           .AllowAnyHeader();
                });
            });
            #endregion

            #region Configure JSON options to handle enum serialization as strings
            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            #endregion

            #region override default authentication middleware to validate token not cookies
            builder.Services.AddAuthentication(options =>
            {
                //Check JWT token header
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                //Change the default redirect url when [Authorize] attribute activated (instead of /Account/Login => /api/Account/Login)
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                //And this option is for all and any other service that don't have a DefaultScheme
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => //verified key
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;  //If you have a Https certification  

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:IssuerIP"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:AudienceIP"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"] ?? ""))
                };
            });
            #endregion

            #region Swagger Setting to enable adding token
            builder.Services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 8 Web API",
                    Description = " ITI Projrcy"
                });
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    new string[] {}
                    }
                    });
            });
            #endregion


            var app = builder.Build();

            // Seed the database
            await app.SeedDatabaseAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowAll");


            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}

