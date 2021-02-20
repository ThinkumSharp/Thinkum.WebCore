/**
* RequestMethod.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/
using System;

namespace Thinkum.WebCore.Endpoints
{
    [Flags]
    public enum RequestMethod : short
    {
        None = 0,
        Get = 1,
        Post = 2,
        Put = 4,
        Delete = 8
    }
}
