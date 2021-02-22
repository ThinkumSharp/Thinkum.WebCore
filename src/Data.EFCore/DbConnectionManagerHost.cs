/**
* DbConnectionManagerHost.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thinkum.WebCore.Data
{
    public class DbConnectionManagerHost : BackgroundService
    {

        protected readonly ILogger logger;
        protected readonly DbConnectionManager mgr;

        public DbConnectionManagerHost(DbConnectionManager mgr, ILogger<DbConnectionManagerHost> logger) : base()
        {
            this.mgr = mgr;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            logger.LogInformation("ExecuteAsync in {0}", this);
            await mgr.ConfigureDataServicesAsync(token);
        }
    }
}
