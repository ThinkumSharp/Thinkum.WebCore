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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            broker.BindEndpoint("/", RequestMethod.Get, async (context) =>
            {
                await context.Response.WriteAsync("Request Successful");
            });

            broker.BindFallbackEndpoint(async (context) =>
            {
                await context.Response.WriteAsync("Fallback Request Successful");
            });

        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // app.UseMiddleware<EndpointBrokerMiddleware>(); // TBD. See src for that class (now defined as abstract)

            app.UseEndpoints(builder =>
            {
                broker.RegisterEndpoints(builder);
            });

        }

    }
}
