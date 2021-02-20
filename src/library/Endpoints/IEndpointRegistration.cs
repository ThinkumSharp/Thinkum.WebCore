/**
* IEndpointRegistration.cs
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
using System.Threading.Tasks;

namespace Thinkum.WebCore.Endpoints
{
    /// <summary>
    /// Interface for registration of a single endpoint, under 
    /// <seealso cref="Thinkum.WebCore.Middleware.EndpointBrokerMiddleware.RegisterEndpoints(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder)"/>
    /// </summary>
    public interface IEndpointRegistration
    {
        /// <summary>
        /// Request routing template, for this endpoint
        /// </summary>
        public string RoutingTemplate { get; }

        /// <summary>
        /// <para>
        /// HTTP request methods to support, for this endpoint. 
        /// </para>
        /// <para>
        /// This value may represent a bitwise OR of the following flags, and should represent at least one of these values:
        /// <list type="bullet">
        ///    <item><seealso cref="RequestMethod.Get"/></item>
        ///    <item><seealso cref="RequestMethod.Post"/></item>
        ///    <item><seealso cref="RequestMethod.Put"/></item>
        ///    <item><seealso cref="RequestMethod.Delete"/></item>
        /// </list>
        /// </para>
        /// </summary>
        public RequestMethod EndpointMethods { get; } // = RequestMethod.Get;

        /// <summary>
        /// Delegate to invoke for this endpoint.
        /// </summary>
        public RequestDelegate EndpointDelegate { get; }
        

        /// <summary>
        ///  Provide any additional configuration for this endpoint.
        /// </summary>
        /// <param name="method">Request method for this call to <c>ConfigureEndpoint</c></param>
        /// <param name="builder">Builder object returned from the <c>Map*</c> call for the provided <c>method</c> on this endpoint</param>
        /// <remarks>
        /// This method will be called after each applicable <c>MapGet</c>, <c>MapPost</c>, <c>MapPut</c>, and <c>MapDelete </c>
        /// in <seealso cref="Thinkum.WebCore.Middleware.EndpointBrokerMiddleware.RegisterEndpoints(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder)"/>, 
        /// respectively with a <c>method</c> equal to one of
        /// <seealso cref="RequestMethod.Get"/>, <seealso cref="RequestMethod.Post"/>, <seealso cref="RequestMethod.Put"/>, 
        /// or <seealso cref="RequestMethod.Delete"/>. Using the provided <c>builder</c>, the implementing method may provide any additional 
        /// configuration for the endpoint, under each corresponding HTTP response method. This may include configuration for CORS policies, 
        /// authorization policies, etc.
        /// </remarks>
        public void ConfigureEndpoint(RequestMethod method, IEndpointConventionBuilder builder);
    }
}