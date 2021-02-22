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
using Microsoft.Extensions.Logging;
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
        protected readonly DbConnectionManager connectionManager;


        #region Properties
        public IConfiguration ServiceConfiguration => config;
        #endregion

        #region Constructors
        public WebCoreDbContext(
                IConfiguration config, DbContextOptions options,
                DbConnectionManager mgr
            ) : base(options)
        {
            this.config = config;
            this.connectionManager = mgr;
        }
        #endregion

        public DbConnectionStringBuilder ConfigureConnectionStringBuilder(DbConnectionStringBuilder builder)
        {
            string name = this.GetType().GetConnectionName();
            var bind = connectionManager.GetConnectionBinding(name);

            var bindContextType = bind.DbContextType;
            if (!this.GetType().IsAssignableFrom(bindContextType))
            {
                string msg = String.Format("DbContext type {0} registered for {1} is not usable with {2}", bindContextType, name, this);
                throw new InvalidOperationException(msg);
            }

            var delg = bind.StringBuilderDelegate;
            delg?.Invoke(name, builder);
            return builder;
        }
    }
}
