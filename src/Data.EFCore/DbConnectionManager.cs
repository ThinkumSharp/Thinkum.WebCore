/**
* DbConnectionManager.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Thinkum.WebCore.Data
{
    public class DbConnectionManager : ConnectionManagerService
    {
        protected readonly ILogger logger;
        protected readonly IServiceProvider services;


        public DbConnectionManager(IServiceProvider services, IOptions<DbConnectionManagerOptions> options, ILogger<DbConnectionManager> logger) : base()
        {
            this.services = services;
            this.logger = logger;
            this.LoadConnectionMap(options.Value);
        }

        public void LoadConnectionMap(DbConnectionManagerOptions options)
        {
            foreach (KeyValuePair<string, DbConnectionBinding> bind in options.ConnectionBindings)
            {
                this.connectionBindings[bind.Key] = bind.Value;
            }
        }

        public DbConnectionBinding GetConnectionBinding(string connectionName)
        {
            DbConnectionBinding binding;
            bool found = connectionBindings.TryGetValue(connectionName, out binding);
            if (found == true)
            {
                return binding;
            }
            else
            {
                // NB Not very obvious when this error may be caused by a mismatch between a value in a class annotation and a value in Startup.cs ...
                string msg = String.Format("No connection binding found for connection name {0}", connectionName);
                throw new InvalidOperationException(msg);
            }
        }

        public async Task ConfigureDataServicesAsync(CancellationToken token)
        {
            using (var scope = services.CreateScope()) 
            {
                foreach (var entry in ConnectionBindings)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    var bind = entry.Value;
                    if (!bind.DatabaseInitialized)
                    {
                        var dbctype = bind.DbContextType;
                        logger.LogInformation("Database initialization for connection {0}", entry.Key); // TBD LogDebug not showing up w/ console
                        using (DbContext dbc = (DbContext)scope.ServiceProvider.GetRequiredService(dbctype)) 
                        {
                            // NB as a side effect, the following should serve to ensure that the database exists
                            await dbc.Database.MigrateAsync(token);
                            await dbc.SaveChangesAsync(token);
                        }
                        bind.DatabaseInitialized = true;
                    }
                }
            }
        }

    }
}