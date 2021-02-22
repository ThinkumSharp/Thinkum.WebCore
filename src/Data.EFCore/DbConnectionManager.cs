/**
* DbConnectionManager.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Thinkum.WebCore.Data
{
    public class DbConnectionManager : ConnectionManagerService, ILoggingService
    {
        protected readonly ILogger logger;

        public DbConnectionManager(IOptions<DbConnectionManagerOptions> options, ILogger<DbConnectionManager> logger) : base()
        {
            this.logger = logger;
            this.LoadConnectionMap(options.Value);
        }

        public void LoadConnectionMap(DbConnectionManagerOptions options)
        {
            foreach (KeyValuePair<string, ConnectionBinding> bind in options.ConnectionBindings)
            {
                this.connectionBindings[bind.Key] = bind.Value;
            }
        }

        public ConnectionBinding GetConnectionBinding(string connectionName)
        {
            ConnectionBinding binding;
            bool found = connectionBindings.TryGetValue(connectionName, out binding);
            if (found == true)
            {
                return binding;
            }
            else
            {
                // NB Not very obvious when this error may be caused by a mismatch between a value in a class annotation and a value in Startup.cs ...
                string msg = String.Format("No connection binding found for connection name {0}", connectionName);
                throw new InvalidOperationException(msg);
            }
        }

        public void LogDebug(string message, params object[]? args)
        {
            logger.LogDebug(message, args);
        }

        public void LogInfo(string message, params object[]? args)
        {
            logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[]? args)
        {
            logger.LogWarning(message, args);
        }

        public void LogError(string message, params object[]? args)
        {
            logger.LogError(message, args);
        }

        public void LogCritical(string message, params object[]? args)
        {
            logger.LogCritical(message, args);
        }
    }
}