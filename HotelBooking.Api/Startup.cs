using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.SqlServer;
using HotelBooking.Api.Helpers;
using HotelBooking.Api.Repositories;
using HotelBooking.MongoDBClient;
using HotelBooking.MongoDBClient.Infrastructures;
using HotelBooking.MongoDBClient.Infrastructures.Interfaces;
using HotelBooking.MongoDBClient.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBooking.Api
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
            services.AddControllers();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hotel Booking.Api - Infusync",
                    Description = "Swagger surface",
                    Contact = new OpenApiContact
                    {
                        Name = "Aduwu Joseph",
                        Email = "aduwujoseph4real@gmail.com",
                        Url = new Uri("http://www.linkedin.com/in/aduwu-joseph-483b91163/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://github.com/AduwuJoseph/HotelBookingApi-Using-Stateless-Machine/LICENSE")
                    }
                });
            });

            RegisterDbDependancies(services);

            // Add Hangfire framework services.
            services.AddHangfire(config =>
            {
                // Read DefaultConnection string from appsettings.json
                var connectionString = Configuration.GetConnectionString("HangfireConnection");
                var storageOptions = new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy()
                    }
                };

                //config.UseLogProvider(new FileLogProvider());
                config.UseColouredConsoleLogProvider(Hangfire.Logging.LogLevel.Info);
                config.UseMongoStorage(connectionString, storageOptions);
            });
            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "default", "notDefault" };
            });
            //services.AddMvc(c => c.EnableEndpointRouting = false);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Booking API v1"));
            }

            app.UseHangfireDashboard();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        public void RegisterDbDependancies(IServiceCollection services)
        {
            //var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //var dbname = Configuration.GetConnectionString("DatabaseName");
            //services.AddSingleton<IDatabaseContext>(new DatabaseContext(
            //    connectionString,
            //    dbname));


            // requires using Microsoft.Extensions.Options
            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<DatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddScoped(typeof(IHotelBookingMongoRepository<>), typeof(HotelBookingMongoRepository<>));

            services.AddSingleton<IHangFireOpRepository, HangFireOpRepository>();
        }
    }
}
