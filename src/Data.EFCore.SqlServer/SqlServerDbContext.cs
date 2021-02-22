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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {

            var cstrbld = ConfigureConnectionStringBuilder();

            string cstr = cstrbld.ConnectionString;
            if (String.IsNullOrEmpty(cstr))
            {
                throw new InvalidOperationException("Invalid connection string");
            }

            builder.UseSqlServer(cstr); // FIXME this represents the only call specific to this class (FIXME Remove this class)
        }
    }
}
