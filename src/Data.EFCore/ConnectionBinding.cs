/**
* ConnectionBinding.cs
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
    public class ConnectionBinding
    {
        protected readonly string connectionName;
        protected readonly Type dbContextType;
        protected readonly Action<string, DbConnectionStringBuilder>? stringBuilderDelegate;

        public string ConnectionName => connectionName;
        public Type DbContextType => dbContextType;
        public Action<string, DbConnectionStringBuilder>? StringBuilderDelegate => stringBuilderDelegate;

        public ConnectionBinding(string connectionName, Type dbContextType, Action<string, DbConnectionStringBuilder>? stringBuilderDelegate)
        {
            this.connectionName = connectionName;
            this.dbContextType = dbContextType;
            this.stringBuilderDelegate = stringBuilderDelegate;
        }

    }
}
