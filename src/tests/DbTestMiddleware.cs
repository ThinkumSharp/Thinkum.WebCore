/**
 * NullMiddleware.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinkum.WebCore.Data;

namespace Thinkum.WebCore
{
    public class DbTestMiddleware
    {

        #region Instance Fields
        protected RequestDelegate? next; // NB support for InvokeAsync implementation
        protected IServiceProvider services;
        protected bool dbInitialized = false;
        #endregion

        #region Instance Constructors
        public DbTestMiddleware(RequestDelegate? next, IServiceProvider services)
        {
            this.next = next;
            this.services = services;
        }
        #endregion

        #region Instance Methods
        public void InvokeNext(HttpContext context)
        {
            // NB support for InvokeAsync implementation
            next?.Invoke(context);
        }
        #endregion

        #region Instance Methods

        public async Task InvokeAsync(HttpContext context)
        {
            // NB
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-5

            // NB the following middleware-based oprn is why the constructor was updated to include IServiceProvider
            using (IServiceScope inScope = services.CreateScope())
            {

                // FIXME hard-coding dbcontext class here, only to access an instance of the correponding dbcontext type
                //
                // FIXME call this for every connection registered under DbConnectionManager, and store the "initialized = true" flag there
                // or simply provide some other development-env code and installation-time scripting to ensure that the database is 
                // created, under every installation
                var dbc = inScope.ServiceProvider.GetRequiredService<MainDbContext>();

                // NB
                if (dbInitialized == false)
                {
                    RelationalDatabaseCreator dbCreator = (RelationalDatabaseCreator)dbc.Database.GetService<IDatabaseCreator>();
                    dbCreator.EnsureCreated();

                    // await dbc.AddAsync("Frob");
                    await dbc.SaveChangesAsync();
                    dbInitialized = true;
                }
            }
            InvokeNext(context);
        }
        #endregion
    }
}
