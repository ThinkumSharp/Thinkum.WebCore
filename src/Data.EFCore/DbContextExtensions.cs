/**
 * DbContextExtensions.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Common;

namespace Thinkum.WebCore.Data
{
    public static class DbContextExtensions
    {

        public static IServiceCollection AddNamedDbContext<TContext>(
            this IServiceCollection services, string connectionName, Action<DbContextOptionsBuilder>? optionsAction = null,
             Action<string, DbConnectionStringBuilder>? connectionStringDelegate = null,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped
            )
            where TContext : WebCoreDbContext
        {
            return services.AddDbContext<TContext>((provider, builder) =>
            {
                // FIXME this name-based connection matching may need to be refactored
                // towards a database-per-class model
                //
                // FIXME this extension method is failing in application - see below

                // -- initial prototype for one application of a DbConnectionManager service
                // var mgr = provider.GetRequiredService<DbConnectionManager>();
                // var cstr = mgr.GetConnectionString(connectionName);
                // builder.cannot.configure.context(fail);

                var ext = new WebCoreOptionsExtension(connectionName, connectionStringDelegate);
                // FIXME is the WithExtension return value to be stored somewhere, or returned from some special delegate form?
                builder.Options.WithExtension<WebCoreOptionsExtension>(ext); // FIXME though specified here, this extension cannot be located later?
                optionsAction?.Invoke(builder);
            }, contextLifetime, optionsLifetime);
        }

    }
}
