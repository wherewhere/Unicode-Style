// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NETMICROFRAMEWORK || SILVERLIGHT || WINDOWSPHONE7_0
namespace System.Reflection
{
    /// <summary>
    /// Defines a key/value metadata pair for the decorated assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class AssemblyMetadataAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMetadataAttribute"/> class with the specified key and value.
        /// </summary>
        /// <param name="key">The metadata key.</param>
        /// <param name="value">The metadata value.</param>
        public AssemblyMetadataAttribute(string key, string? value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets the metadata key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets the metadata value.
        /// </summary>
        public string? Value { get; }
    }
}
#endif