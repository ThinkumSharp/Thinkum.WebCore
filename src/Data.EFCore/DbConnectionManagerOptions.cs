/**
* DbConnectionManagerOptions.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Thinkum.WebCore.Data
{

    public class DbConnectionManagerOptions : ConnectionManagerService
    {
        public DbConnectionManagerOptions() : base()
        {
        }

        public void MapDataService(
            string connectionName,
            Type dbContextType,
            Type stringBuilderType, 
            string? connectionStringTemplate = null, 
            Action<string, DbConnectionStringBuilder>? stringBuilderDelegate = null
            )
        {
            var binding = new ConnectionBinding(connectionName, dbContextType, stringBuilderType, connectionStringTemplate, stringBuilderDelegate);
            MapDataService(binding!);
        }

        public void MapDataService(ConnectionBinding binding)
        {
            var name = binding.ConnectionName;
            connectionBindings[name] = binding;
        }

    }
}
