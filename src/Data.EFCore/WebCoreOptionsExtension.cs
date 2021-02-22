/**
 * WebCoreOptionsExtension.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace Thinkum.WebCore.Data
{

    internal class WebCoreOptionsExtensionInfo : DbContextOptionsExtensionInfo
    {

        public override bool IsDatabaseProvider { get => false; }

        public override string LogFragment => throw new System.NotImplementedException();

        public WebCoreOptionsExtensionInfo([NotNull] IDbContextOptionsExtension extension) : base(extension)
        {

        }

        public override long GetServiceProviderHashCode()
        {
            return 0;
        }

        public override void PopulateDebugInfo([NotNull] IDictionary<string, string> debugInfo)
        {
            var ext = base.Extension;
            if (ext is WebCoreOptionsExtension wcext)
                debugInfo.Add("WebCoreOptionsExtension:name", wcext.ConnectionName);
        }
    }

    public class WebCoreOptionsExtension : IDbContextOptionsExtension
    {

        protected readonly string connectionName;
        protected readonly Action<string, DbConnectionStringBuilder>? connectionStringDelegate;
        protected DbContextOptionsExtensionInfo? cachedInfo = null;

        public string ConnectionName => connectionName;

        public DbContextOptionsExtensionInfo Info
        {
            get
            {
                if (cachedInfo == null)
                {
                    cachedInfo = new WebCoreOptionsExtensionInfo(this);
                }
                return cachedInfo;
            }
        }

        public WebCoreOptionsExtension(string connectionName, Action<string, DbConnectionStringBuilder>? connectionStringDelegate = null) : base()
        {
            this.connectionName = connectionName;
            this.connectionStringDelegate = connectionStringDelegate;
        }

        public void ApplyServices(IServiceCollection services)
        {
            // NB N/A
        }

        public void Validate(IDbContextOptions options)
        {
            // NB N/A
        }
    }
}
