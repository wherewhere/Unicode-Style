﻿#if NETMICROFRAMEWORK
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.All, Inherited = true)]
    internal sealed class CompilerGeneratedAttribute : Attribute
    {
        public CompilerGeneratedAttribute() { }
    }
}
#endif