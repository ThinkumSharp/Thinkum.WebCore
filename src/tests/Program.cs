/**
 * Program.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Thinkum.WebCore
{
    internal static class ApplicationConstants
    {
        // NB AppName is used throughout this test, for purposes of database connection string matching.
        // The value may also be used to provide some database metadata and for data file naming.
        //
        // As it will be used as a database name, non-alphanumeric characters in the AppName string may be inadvisable
        internal const string AppName = "WebCoreTests";
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder<MainDbContext>(ApplicationConstants.AppName, args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder<TContext>(string appName, string[] args)
            where TContext: DbContext
        {
            return Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup((webbuilder) =>
                     {
                         var config = webbuilder.Configuration;
                         return new Startup<TContext>(appName, config);
                     });
                 });
        }
    }
}
