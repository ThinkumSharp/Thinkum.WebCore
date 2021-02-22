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
            // N/A
        }
    }

    public class WebCoreOptionsExtension : IDbContextOptionsExtension
    {
        protected readonly Action<DbConnectionStringBuilder>? connectionStringDelegate;
        protected DbContextOptionsExtensionInfo? cachedInfo = null;

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

        public WebCoreOptionsExtension(Action<DbConnectionStringBuilder>? connectionStringDelegate = null) : base()
        {
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
