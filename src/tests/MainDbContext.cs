/**
 * MainDbContext.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinkum.WebCore.Data;

namespace Thinkum.WebCore
{
    [DataConnection(ApplicationConstants.MainDbConnection)] // NB coupled to a connection name bound under Startup.cs
    public class MainDbContext : SqlServerDbContext
    {

        // TBD adding DbList<...>; decoupling this to the DBMS implementation (delegates)

        public MainDbContext(
            IConfiguration config, DbContextOptions options,
            DbConnectionManager mgr
            ) : base(config, options, mgr)
        {

        }
    }
}
