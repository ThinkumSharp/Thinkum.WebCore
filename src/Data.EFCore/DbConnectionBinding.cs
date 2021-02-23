/**
* DbConnectionBinding.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/
using System;
using System.Data.Common;

namespace Thinkum.WebCore.Data
{
    public class DbConnectionBinding
    {
        // NB used principally under WebCoreDbContext.ConfigureConnectionStringBuilder(...)

        // NB constant values for GetStringBuilderPrototype()
        protected static readonly Type[] stringCtorSignature = new Type[] { typeof(String) };
        protected static readonly Type[] nullCtorSignature = new Type[] { };
        protected static readonly object[] nullArgs = new object[] { };

        protected readonly string connectionName;
        private readonly string? connectionStringTemplate;
        protected readonly Type dbContextType;
        protected readonly Type stringBuilderType;
        protected DbConnectionStringBuilder? stringBuilderPrototype = null;
        protected readonly Action<string, DbConnectionStringBuilder>? stringBuilderDelegate;

        public string ConnectionName => connectionName;
        public Type DbContextType => dbContextType;
        public Action<string, DbConnectionStringBuilder>? StringBuilderDelegate => stringBuilderDelegate;

        public bool DatabaseInitialized { get; internal set; }

        public string? ConnectionStringTemplate => connectionStringTemplate;

        public DbConnectionBinding(
                string connectionName, Type dbContextType, Type stringBuilderType,
                string? template = null,
                Action<string, DbConnectionStringBuilder>? stringBuilderDelegate = null
            )
        {
            // FIXME provide stringBuilderType and connectionStringTemplate in the CTOR - also to what calls this ctor
            this.connectionName = connectionName;
            this.dbContextType = dbContextType;
            this.stringBuilderType = stringBuilderType;
            this.connectionStringTemplate = template;
            this.stringBuilderDelegate = stringBuilderDelegate;
        }

        public DbConnectionStringBuilder GetStringBuilderPrototype()
        {
            string? template = this.connectionStringTemplate;
            if (stringBuilderPrototype == null)
            {
                Type[] signature;
                if (template == null)
                    signature = nullCtorSignature;
                else
                    signature = stringCtorSignature;

                var protoCtor = stringBuilderType.GetConstructor(signature);
                object[] args;
                if (template == null)
                    args = nullArgs;
                else
                    args = new object[] { template! };

                DbConnectionStringBuilder inst = (DbConnectionStringBuilder)protoCtor.Invoke(args);
                stringBuilderPrototype = inst;
            }
            else if (template != null)
            {
                stringBuilderPrototype.ConnectionString = template;
            }
            return stringBuilderPrototype;
        }

        public string GetConnectionString()
        {
            // NB convenience method
            //
            // NB under database system configurations requiring credentials in the connection string, 
            // the string returned from this method may contain sensitive data
            var proto = GetStringBuilderPrototype();
            ConfigureStringBuilder(proto);
            return proto.ConnectionString;
        }

        public DbConnectionStringBuilder? ConfigureStringBuilder(DbConnectionStringBuilder builder)
        {
            if (this.stringBuilderDelegate == null)
            {
                return null; // no-op
            }
            else
            {
                this.stringBuilderDelegate!(this.connectionName, builder);
                return builder;
            }
        }
    }
}
