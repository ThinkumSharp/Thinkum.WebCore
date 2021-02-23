/**
 * Startup.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Thinkum.WebCore.Data;
using Thinkum.WebCore.Endpoints;

namespace Thinkum.WebCore
{

    public class Startup<TContext, TBuilder>
        where TContext : DbContext
        where TBuilder : DbConnectionStringBuilder
    {

        protected static readonly Type dbStringBuilderType = typeof(TBuilder);

        protected EndpointBroker broker;
        protected readonly string appName;
        protected readonly IConfiguration config;

        protected string AppName => appName;

        public Startup(string appName, IConfiguration config)
        {
            // TBD DI for this constructor - note how it's being called now, under a factory methodology in Program.cs
            this.appName = appName;
            this.config = config;
            this.broker = new EndpointBroker();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHostedService<DbConnectionManagerHost>(); // NB initial database migration, db creation by side effect, once per connection
            // FIXME supplemental to the following, provide a simpler utility using templates and formatting for connection strings
            // - app data directory
            // - credentials
            // - instance address
            // - database name
            // - etc
            services.AddSingleton<DbConnectionManager>().AddOptions<DbConnectionManagerOptions>()
                .Configure(options => this.ConfigureConnectionManagerOptions(options));
            services.AddDbContext<TContext>();

            // TBD passing route metadata to endpoint instances => db connection name per endpoint
            broker.BindEndpoint("/", RequestMethod.Get,
                async (context) =>
                {
                    string msg = String.Format("Response Successful, for endpoint \"{0}\"", context.GetEndpoint()!.DisplayName ?? "(TBD)");
                    await context.Response.WriteAsync(msg);
                });

            broker.BindFallbackEndpoint(
                async (context) =>
                {
                    string msg = String.Format("Fallback Response Successful, for request ({0}) to path \"{1}\" with endpoint \"{2}\"",
                        context.Request.Method, context.Request.Path.Value, context.GetEndpoint()!.DisplayName ?? "(TBD)");
                    await context.Response.WriteAsync(msg);
                },
                (builder) =>
                {
                    builder.WithDisplayName("Fallback");
                });
        }

        private void ConfigureConnectionManagerOptions(DbConnectionManagerOptions options)
        {
            string? template = config.GetConnectionString(this.AppName); // FIXME test this rudimentary templating
            if (String.IsNullOrEmpty(template))
                template = null;

            options.MapDataService(this.AppName, typeof(TContext), dbStringBuilderType, template, this.ConfigureConnectionString);
        }

        private void ConfigureConnectionString(string name, DbConnectionStringBuilder builder)
        {
            // NB coupled to the selection of database implementation

            if (builder is SqlConnectionStringBuilder sqlBuilder)
            {
                // var dataFolder = Path.Combine(Environment.CurrentDirectory, "Data"); // FIXME ensure directory exists

                // NB ensure that the data file is not created under %HOME%
                // FIXME implement a GetDataFolder(Application) ... method
                var dataFolder = Environment.CurrentDirectory; // NB only for testing -- should be set at installation time
                var mdfName = name + ".mdf";
                sqlBuilder.AttachDBFilename = Path.Combine(dataFolder, mdfName); // FIXME use special folders to compute an absolute pathname

                sqlBuilder.IntegratedSecurity = true;
                sqlBuilder.MultipleActiveResultSets = true;
                sqlBuilder.ApplicationName = name;

                sqlBuilder.DataSource = @"(localdb)\MSSQLLocalDb"; // NB this instance name works, locally

                // sqlBuilder.DataSource = @"np:\\.\pipe\LOCALDB#FE1DA647\tsql\query";
                // ^ NB via 'SqlLocalDB info' cmd
                // cf. https://stackoverflow.com/questions/10214688/why-cant-i-connect-to-a-sql-server-2012-localdb-shared-instance

                sqlBuilder.InitialCatalog = name; // i.e 'Database' keyword
            }
            else
            {
                string msg = String.Format("Unsupported connection string builder: {0}", builder);
                throw new InvalidOperationException(msg);
            }

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // app.UseMiddleware<DbTestMiddleware>();

            app.UseEndpoints(builder =>
            {
                broker.RegisterEndpoints(builder);
            });

        }

    }
}
