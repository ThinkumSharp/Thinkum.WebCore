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
    internal static class ApplicationConstants
    {
        internal const string MainDbConnection = "WebCoreTests"; // NB Dots in this string may be inadvisable
    }

    public class Startup
    {

        protected EndpointBroker broker;
        protected IConfiguration config;

        public Startup(IConfiguration config)
        {
            // TBD DI for this constructor
            this.config = config;
            this.broker = new EndpointBroker();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<DbConnectionManager>().AddOptions<DbConnectionManagerOptions>().Configure((options) =>
            {
                options.MapDataService(ApplicationConstants.MainDbConnection, typeof(MainDbContext), // ConfigureConnectionString, // NB TO DO
                    (name, builder) => // name: string, builder: DbConnectionStringBuilder
                    {
                        // TBD this section is untested (FIXME WithExtensions may be useless for end users of EF Core)
                        if (builder is SqlConnectionStringBuilder sqlBuilder)
                        {
                            // var dataFolder = Path.Combine(Environment.CurrentDirectory, "Data"); // NB only for testing (FIXME ensure directory exists)

                            // ensure that the data file is not created under %HOME%
                            var dataFolder = Environment.CurrentDirectory; // NB only for testing -- should be set at installation time
                            var mdfName = name + ".mdf";
                            sqlBuilder.AttachDBFilename = Path.Combine(dataFolder, mdfName); // FIXME use special folders to compute an absolute pathname

                            sqlBuilder.IntegratedSecurity = true;
                            sqlBuilder.MultipleActiveResultSets = true;
                            sqlBuilder.ApplicationName = name;

                            sqlBuilder.DataSource = @"(localdb)\MSSQLLocalDb"; // works

                            // sqlBuilder.DataSource = @"np:\\.\pipe\LOCALDB#FE1DA647\tsql\query";
                            // ^ see https://stackoverflow.com/questions/10214688/why-cant-i-connect-to-a-sql-server-2012-localdb-shared-instance

                            // sqlBuilder.DataSource = @"'(localdb)\ProjectsV13"; // NB DNW at here

                            sqlBuilder.InitialCatalog = name; // i.e 'Database' keyword
                        }
                        else
                        {
                            string msg = String.Format("Unsupported connection string builder: {0}", builder);
                            throw new InvalidOperationException(msg);
                        }
                    }
                    );
            });

            services.AddDbContext<MainDbContext>();


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

        /* // TO DO
        private void ConfigureConnectionString(string name, DbConnectionStringBuilder builder)
        {
            throw new NotImplementedException(); // NB see above
            // NB coupled to the selection of database implementation
        }
        */

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMiddleware<DbTestMiddleware>();

            // app.UseMiddleware<EndpointBrokerMiddleware>(); // TBD. See src for that class (now defined as abstract)

            app.UseEndpoints(builder =>
            {
                broker.RegisterEndpoints(builder);
            });

        }

    }
}
