/**
* ILoggingService.cs
* Copyright (C) 2021 Sean Champ
*
* This Source Code is subject to the terms of the Mozilla Public
* License v. 2.0 (MPL). If a copy of the MPL was not distributed
* with this file, you can obtain one at http://mozilla.org/MPL/2.0/
*
*/
namespace Thinkum.WebCore.Data
{
    public interface ILoggingService
    {
        void LogDebug(string message, params object[]? args);
        void LogInfo(string message, params object[]? args);
        void LogWarning(string message, params object[]? args);
        void LogError(string message, params object[]? args);
        void LogCritical(string message, params object[]? args);
    }
}