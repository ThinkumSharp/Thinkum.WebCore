/**
 * WebCoreDbContext.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Thinkum.WebCore.Data
{

    public abstract class WebCoreDbContext : DbContext
    {
        // NB should be added via DbContextExtensions.AddNamedDbContext(string ....)
        // such that should ensure that a WebCoreOptionsExtension is registered to the context's options,
        // encapsulating the provided string

        protected readonly IConfiguration config;
        protected readonly string connectionName;
        protected readonly Action<string, DbConnectionStringBuilder>? connectionStringDelegate;


        #region Properties
        public IConfiguration ServiceConfiguration => config;
        public string ConnectionName => connectionName;
        #endregion

        #region Constructors
        public WebCoreDbContext(IConfiguration config, DbContextOptions options, Action<string, DbConnectionStringBuilder>? connectionStringDelegate = null) : base(options)
        {
            this.config = config;
            this.connectionStringDelegate = connectionStringDelegate;
            try
            {
                var ext = options.GetExtension<WebCoreOptionsExtension>();
                // NB Note the connectionName's usage under e.g SqlServerDbContext.OnConfiguring()
                connectionName = ext.ConnectionName;
            }
            catch (InvalidOperationException exc)
            {
                // FIXME reached
                string msg = String.Format("Unable to locate extension WebCoreOptionsExtension in provided DbContextOptions {0}", options);
                throw new InvalidOperationException(msg, exc);
            }
        }
        #endregion

        protected void ConfigureConnectionString(string connectionName, DbConnectionStringBuilder builder)
        {
            connectionStringDelegate?.Invoke(connectionName, builder);
        }

    }
}
