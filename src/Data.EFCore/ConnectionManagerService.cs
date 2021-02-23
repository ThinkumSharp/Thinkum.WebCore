/**
* ConnectionManagerService.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Thinkum.WebCore.Data
{
    public abstract class ConnectionManagerService
    {

        protected readonly Dictionary<string, DbConnectionBinding> connectionBindings;
        public Dictionary<string, DbConnectionBinding> ConnectionBindings => connectionBindings;

        public ConnectionManagerService()
        {
            connectionBindings = new Dictionary<string, DbConnectionBinding>(1);
        }

    }
}