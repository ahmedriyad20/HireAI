using HireAI.API.Extensions;
using HireAI.Data.Models.Identity;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.Mappings;
using HireAI.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Add Swagger generation service
            builder.Services.AddSwaggerGen();
            //builder.Services.AddDbContext<HireAIDbContext>();

            #region Register DbContext and Services in the DI Container
            builder.Services.AddDbContext<HireAIDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("HireAiDB"));
            });

            //REGISTER ApplicationUser and IdentityRole with the DI Container
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;

                //Lockout Settings
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddEntityFrameworkStores<HireAIDbContext>()
                .AddDefaultTokenProviders();

            // Register repositories and services using extension methods
            builder.Services.AddApplicationRepositories();
            builder.Services.AddApplicationServices();
            #endregion

            #region Add AutoMapper service
            builder.Services.AddAutoMapper(cfg => { },  typeof(ApplicationProfile).Assembly);
            #endregion

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

            #region Seeding the Database
            // Apply migrations and seed the database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    var context = services.GetRequiredService<HireAIDbContext>();

                    // Check if there are any pending migrations first (safe & fast)
                    var pending = await context.Database.GetPendingMigrationsAsync();
                    if (pending.Any())
                    {
                        logger.LogInformation("Applying {Count} pending migrations...", pending.Count());
                        await context.Database.MigrateAsync();
                        logger.LogInformation("Database migrated successfully.");

                    }
                    else
                    {
                        logger.LogInformation("No pending migrations.");
                    }

                    // Optional: call your seed method here
                    await DbSeeder.SeedAsync(services);
                    logger.LogInformation("Database seeding finished.");

                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                    // In production you might rethrow or stop startup depending on policy.
                    // throw;
                }
            }
            #endregion



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
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

