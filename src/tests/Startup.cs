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
            // NB Note how the OnConfiguring method for SqlServerDbContext is wired to use the provided connection name
            // as a key onto the table of ConnectionStrings e.g under appsettings.json
            //
            // FIXME Need an app for testing the wiring of this DbContext extension under dependency injection
            services.AddNamedDbContext<SqlServerDbContext>("WebCore.Tests" , null,
                (name, builder) => {
                    if (builder is SqlConnectionStringBuilder sqlBuilder)
                    {
                        // var dataFolder = Path.Combine(Environment.CurrentDirectory, "Data"); // NB only for testing (FIXME ensure directory exists)
                        var dataFolder = Environment.CurrentDirectory; // NB only for testing -- should be set at installation time
                        var mdfName = name + ".mdf"; // TBD file is not being created
                        sqlBuilder.AttachDBFilename = Path.Combine(dataFolder, mdfName); // FIXME use special folders to compute an absolute pathname
                        sqlBuilder.IntegratedSecurity = true;
                        sqlBuilder.MultipleActiveResultSets = true;
                        sqlBuilder.ApplicationName = name;
                        sqlBuilder.DataSource = @".\\sqlexpress";
                        sqlBuilder.InitialCatalog = name; // i.e 'Database' keyword
                    } else
                    {
                        string msg = String.Format("Unrecognized type of sqlBuilder: {0}", builder);
                        throw new InvalidOperationException(msg);
                    }
                });

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
