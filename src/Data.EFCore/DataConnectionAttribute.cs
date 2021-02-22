/**
* DataConnectionAttribute.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/

using System;

namespace Thinkum.WebCore.Data
{

    public static class DataConnectionExtensions
    {
        public static string GetConnectionName(this Type whence)
        {
            var attrs = whence.GetCustomAttributes(true);
            string? name = null;

            foreach (var att in attrs)
            {
                if (att is DataConnectionAttribute datt)
                {
                    name = datt.ConnectionName;
                }
            }
            if (name == null)
            {
                name = whence.Name;
            }
            return name;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DataConnectionAttribute : Attribute
    {
        protected readonly string connectionName;
        public string ConnectionName => connectionName;

        public DataConnectionAttribute(string? connectionName = null) : base()
        {
            if (connectionName == null)
            {
                this.connectionName = this.GetType().Name;
            }
            else
            {
                this.connectionName = connectionName;
            }
        }
    }
}