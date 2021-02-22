/**
 * SqlServerDbContext.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinkum.WebCore.Data
{

    public class SqlServerDbContext : WebCoreDbContext
    {
        public SqlServerDbContext(IConfiguration config, DbContextOptions<SqlServerDbContext> options) : base(config, options)
        {
            // FIXME provide the "data service name" when initializing the instance,
            // then use that "data service name" as the connection string name, below
            //
            // Demonstrate this under testing - implement the handling in WebCoreDbContext
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            // FIXME this shortcuts any call to a connection string builder && TBD credentials handling
            var cstr = config.GetConnectionString(this.connectionName); // NB The main reason why DbContextExtensions, ...
            var cstrbld = new SqlConnectionStringBuilder(cstr);
            ConfigureConnectionString(this.connectionName, cstrbld);
            builder.UseSqlServer(cstrbld.ConnectionString);

            RelationalDatabaseCreator frob = (RelationalDatabaseCreator)this.Database.GetService<IDatabaseCreator>();
            frob.EnsureCreated();
        }
    }
}
