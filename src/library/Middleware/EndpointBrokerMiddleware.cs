/**
 * EndpointBrokerMiddleware.cs
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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Thinkum.WebCore.Endpoints;

namespace Thinkum.WebCore.Middleware
{
    public abstract class EndpointBrokerMiddleware : EndpointBroker
    {

        // NB prototype - extending EndtpointBroker as middleware

        #region Instance Fields
        protected RequestDelegate? next; // NB support for InvokeAsync implementation
        #endregion

        #region Instance Constructors
        public EndpointBrokerMiddleware(RequestDelegate? next)
        {
            this.next = next;
        }
        #endregion

        #region Instance Methods
        public void InvokeNext(HttpContext context)
        {
            // NB support for InvokeAsync implementation
            next?.Invoke(context);
        }
        #endregion

        #region Abstract Instance Methods

        public abstract Task InvokeAsync(HttpContext context);
        // NB
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-5

        #endregion

    }
}