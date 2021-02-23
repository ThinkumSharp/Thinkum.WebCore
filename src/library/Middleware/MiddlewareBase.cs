/**
 * MiddlewareBase.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thinkum.WebCore.Middleware
{
    public abstract class MiddlewareBase
    {

        #region Instance Fields
        protected readonly RequestDelegate? next;
        protected readonly IServiceProvider services;
        #endregion

        #region Instance Constructors
        public MiddlewareBase(RequestDelegate? next, IServiceProvider services)
        {
            this.next = next;
            this.services = services;
        }
        #endregion

        #region Instance Methods
        public async Task InvokeNextAsync(HttpContext context)
        {
            // NB support for InvokeAsync implementation
            if (next != null)
                await next!.Invoke(context);
        }
        #endregion

        #region Abstract Instance Methods
        public abstract Task InvokeAsync(HttpContext context);
        #endregion
    }
}
