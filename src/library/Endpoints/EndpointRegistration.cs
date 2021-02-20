/**
 * EndpointRegistration.cs
 * Copyright (C) 2021 Sean Champ
 *
 * This Source Code is subject to the terms of the Mozilla Public
 * License v. 2.0 (MPL). If a copy of the MPL was not distributed
 * with this file, you can obtain one at http://mozilla.org/MPL/2.0/
 *
 */
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace Thinkum.WebCore.Endpoints
{
    public class EndpointRegistration : IEndpointRegistration
    {

        private readonly string template;
        private readonly RequestMethod methods;
        private readonly RequestDelegate endpointDelegate;
        private readonly Action<RequestMethod, IEndpointConventionBuilder>? configureAction;

        public string RoutingTemplate => template;
        public RequestMethod EndpointMethods => methods;
        public RequestDelegate EndpointDelegate => endpointDelegate;
        public Action<RequestMethod, IEndpointConventionBuilder>? ConfigureAction => configureAction; // NB support for an optional ConfigureEndpoint action


        public EndpointRegistration(string template, RequestMethod methods, RequestDelegate endpointDelegate,
                                                    Action<RequestMethod, IEndpointConventionBuilder>? configureAction = null)
        {
            this.template = template;
            this.methods = methods;
            this.endpointDelegate = endpointDelegate;
            this.configureAction = configureAction;
        }

        public void ConfigureEndpoint(RequestMethod method, IEndpointConventionBuilder builder)
        {
            // Note the optional 'configure' parameter in EndpointBroker.BindEndpoint(...)
            if (ConfigureAction != null)
            {
                ConfigureAction.Invoke(method, builder);
            }
        }
    }
}