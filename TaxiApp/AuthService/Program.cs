using AuthService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using AuthService.repositories;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using AuthService.Services;
using Microsoft.AspNetCore.Identity;

namespace AuthService
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                var services = new ServiceCollection();

               /* services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer("DevDB"));*/

                services.AddCors(options =>
                {
                    options.AddPolicy("ReactApp",
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:3000") // specify allowed origins
                                   .AllowAnyHeader()
                                   .AllowAnyMethod();
                        });
                });

                // Register IUserService with its implementation

                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 12;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;

                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                    options.User.RequireUniqueEmail = false;

                });
                //services.AddIdentityApiEndpoints<IdentityUser>(); 
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

                services.AddControllers();

                // Add authorization services
                services.AddAuthentication();
                services.AddAuthorization();

                var serviceProvider = services.BuildServiceProvider();

                // The ServiceManifest.XML file defines one or more service type names.
                // Registering a service maps a service type name to a .NET type.
                // When Service Fabric creates an instance of this service type,
                // an instance of the class is created in this host process.




                ServiceRuntime.RegisterServiceAsync("AuthServiceType",
                    context => new AuthService(context, serviceProvider)).GetAwaiter().GetResult();



                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(AuthService).Name);


                // Run the web host
                var host = Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.ConfigureServices(s =>
                        {
                            // Add services directly, using the previously built service provider
                            s.AddSingleton(serviceProvider);

                            // Alternatively, re-register services by iterating over the service provider's services
                            foreach (var descriptor in services)
                            {
                                s.Add(descriptor);
                            }
                        })
                        .Configure(app =>
                        {
                            // Use CORS with the specified policy
                            app.UseCors("ReactApp");

                            // Other middleware
                            app.UseRouting();
                            app.UseAuthentication();
                            app.UseAuthorization();

                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllers();
                            });
                        });
                    })
                    .Build();

                host.Run();


                // Prevents this host process from terminating so services keeps running. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }


}
