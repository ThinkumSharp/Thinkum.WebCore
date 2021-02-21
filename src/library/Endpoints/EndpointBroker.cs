/**
* EndpointBroker.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Thinkum.WebCore.Endpoints
{
    public class EndpointBroker
    {
        // NB Example available in Thinkum.WebCore.Tests/Startup.cs

        // NB It may not be necessary to provide any UnbindEndpoint methods, considering the overall
        // non-dynamic nature of some esimated usage cases for this class. 

        protected List<IEndpointRegistration> endpoints;

        protected RequestDelegate? fallbackDelegate = null;

        protected Action<IEndpointConventionBuilder>? fallbackConfig = null;

        public EndpointBroker()
        {
            endpoints = new List<IEndpointRegistration>();
        }

        public void BindEndpoint(string template, RequestMethod methods, RequestDelegate delg,
                    Action<RequestMethod, IEndpointConventionBuilder>? configure = null)
        {
            var reg = new EndpointRegistration(template, methods, delg, configure);
            BindEndpoint(reg);
        }

        public void BindEndpoint(IEndpointRegistration reg)
        {
            endpoints.Add(reg);
        }

        public void BindFallbackEndpoint(RequestDelegate fallbackDelegate, Action<IEndpointConventionBuilder>? fallbackConfig = null)
        {
            this.fallbackDelegate = fallbackDelegate;
            this.fallbackConfig = fallbackConfig;
        }

        public void RegisterEndpoints(IEndpointRouteBuilder builder)
        {
            // called e.g in Startup.Configure(app, env) ... app.UseEndpoints(builder  => {  broker.RegisterEndpoints(builder); })

            if (fallbackDelegate != null)
            {
                var cbuilder = builder.MapFallback(fallbackDelegate);
                ConfigureFallback(cbuilder);
            }

            foreach (IEndpointRegistration reg in endpoints)
            {
                string path = reg.RoutingTemplate;
                RequestDelegate delg = reg.EndpointDelegate;
                RequestMethod method = reg.EndpointMethods;

                if (method.HasFlag(RequestMethod.Get))
                {
                    var cbuilder = builder.MapGet(path, delg);
                    reg.ConfigureEndpoint(RequestMethod.Get, cbuilder);
                }
                if (method.HasFlag(RequestMethod.Post))
                {
                    var cbuilder = builder.MapPost(path, delg);
                    reg.ConfigureEndpoint(RequestMethod.Post, cbuilder);
                }
                if (method.HasFlag(RequestMethod.Put))
                {
                    var cbuilder = builder.MapPut(path, delg);
                    reg.ConfigureEndpoint(RequestMethod.Put, cbuilder);
                }
                if (method.HasFlag(RequestMethod.Delete))
                {
                    var cbuilder = builder.MapDelete(path, delg);
                    reg.ConfigureEndpoint(RequestMethod.Delete, cbuilder);
                }
            }
        }

        private void ConfigureFallback(IEndpointConventionBuilder cbuilder)
        {
            if (fallbackConfig != null)
            {
                fallbackConfig(cbuilder);
            }
        }
    }
}
