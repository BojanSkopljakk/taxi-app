using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using AuthenticationService.Data;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using AuthenticationService.Seeding;
using AuthenticationService.Models;

namespace AuthenticationService
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance.
    /// </summary>
    internal sealed class AuthenticationService : StatelessService
    {
        public AuthenticationService(StatelessServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");



                        var builder = WebApplication.CreateBuilder();

                        builder.Services.AddSingleton<StatelessServiceContext>(serviceContext);
                        builder.WebHost
                                    .UseKestrel()
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url);
                        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                            options.UseSqlServer(builder.Configuration.GetConnectionString("DevDB")));
                        builder.Services.AddCors(options =>
                        {
                            options.AddPolicy("ReactApp", policyBuilder =>
                            {
                                policyBuilder.WithOrigins("http://localhost:3000");
                                policyBuilder.AllowAnyHeader();
                                policyBuilder.AllowAnyMethod();
                                policyBuilder.AllowCredentials();
                            }
                            );
                            
                        });
                        builder.Services.AddScoped<IUserRepository, SQLUserRepository>();
                        builder.Services.AddScoped<ITokenRepository, TokenRepository>();
                        builder.Services.AddScoped<IDriverApplicationRepository, DriverApplicationRepository>();
                        builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
                        builder.Services.AddControllers();
                        builder.Services.AddEndpointsApiExplorer();
                        builder.Services.AddSwaggerGen();
                       // builder.Services.AddIdentityCore<IdentityUser>()
                       // .AddRoles<IdentityRole>()
                        //.AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("AuthenticationService")
                        //.AddEntityFrameworkStores<ApplicationDbContext>()
                        //.AddDefaultTokenProviders();

                        

                        builder.Services.Configure<IdentityOptions>(options =>
                        {
                            options.Password.RequireDigit = false;
                            options.Password.RequireLowercase = false;
                            options.Password.RequireNonAlphanumeric = false;
                            options.Password.RequireUppercase = false;
                            options.Password.RequireLowercase = false;
                            options.Password.RequiredLength = 6;
                            options.Password.RequiredUniqueChars = 1;
                        
                        });

                        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                            options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                                ValidAudience = builder.Configuration["Jwt:Audience"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                            });

                        var app = builder.Build();
                        if (app.Environment.IsDevelopment())
                        {
                        app.UseSwagger();
                        app.UseSwaggerUI();
                        }
                        app.UseCors("ReactApp");
                        app.UseAuthentication();
                        app.UseAuthorization();
                        app.MapControllers();
                        //app.UseCors("ReactApp");

                    using (var scope = app.Services.CreateScope())
                    {
                        var services = scope.ServiceProvider;
                        try
                        {
                            // Call the seeding logic
                            IdentityDataSeeder.SeedRolesAndAdminUserAsync(services).Wait();
                        }
                        catch (Exception ex)
                        {
                            // Handle errors during seeding
                            ServiceEventSource.Current.ServiceMessage(serviceContext, $"Error seeding data: {ex.Message}");
                        }
                    }



                        return app;

                    }))
            };
        }
    }
}
