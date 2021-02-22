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
        // protected DbContext dbContext;
        protected IServiceProvider services;
        #endregion

        #region Instance Constructors
        // FIXME fails to find the local extension for the dbcontext
        /**/
        public DbTestMiddleware(RequestDelegate? next, IServiceProvider services)
        {
            this.next = next;
            // this.dbContext = context;
            this.services = services;
        }
        /**/
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

            using (IServiceScope inScope = services.CreateScope())
            {
                var dbc = inScope.ServiceProvider.GetRequiredService<SqlServerDbContext>();
                await dbc.AddAsync("Frob");
                await dbc.SaveChangesAsync();
            }
            // await dbContext.SaveChangesAsync();
            InvokeNext(context);
        }
        #endregion
    }
}
