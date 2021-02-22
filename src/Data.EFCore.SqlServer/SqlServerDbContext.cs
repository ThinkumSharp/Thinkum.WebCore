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
using Microsoft.Extensions.Logging;
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
        public SqlServerDbContext(
            IConfiguration config, DbContextOptions options,
             DbConnectionManager mgr
             ) : base(config, options, mgr)
        {
            // FIXME provide the "data service name" when initializing the instance,
            // then use that "data service name" as the connection string name, below
            //
            // Demonstrate this under testing - implement the handling in WebCoreDbContext
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            var cstrbld = new SqlConnectionStringBuilder();

            // FIXME note the semantics of the connection name handling via DataConnectionAttribute
            // in WebCoreDbContext.ConfigureConnectionStringBuilder(...)
            ConfigureConnectionStringBuilder(cstrbld);

            string cstr = cstrbld.ConnectionString;
            if(String.IsNullOrEmpty(cstr))
            {
                throw new InvalidOperationException("Invalid connection string");
            }

            // FIXME empty string
            builder.UseSqlServer(cstr); // FIXME this is the only call specific to this class - the connection string handling is simply generic

            /* FIXME the following may qualify as a use of this dbcontext. It may throw if called during OnConfiguring 
            RelationalDatabaseCreator frob = (RelationalDatabaseCreator)this.Database.GetService<IDatabaseCreator>();
            frob.EnsureCreated();
            */
        }
    }
}
