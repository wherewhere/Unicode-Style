// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NETMICROFRAMEWORK3_0
namespace System.Runtime.Versioning
{
    /// <summary>
    /// Identifies the version of .NET that a particular assembly was compiled against.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed class TargetFrameworkAttribute : Attribute
    {
        private readonly string _frameworkName;  // A target framework moniker
        private string? _frameworkDisplayName;

        /// <summary>
        /// Initializes an instance of the <see cref="TargetFrameworkAttribute"/> class by specifying the .NET version against which an assembly was built.
        /// </summary>
        /// <param name="frameworkName">The version of .NET against which the assembly was built.</param>
        /// <exception cref="ArgumentNullException"><paramref name="frameworkName"/> is <see langword="null"/>.</exception>
        // The frameworkName parameter is intended to be the string form of a FrameworkName instance.
        public TargetFrameworkAttribute(string frameworkName)
        {
            _frameworkName = frameworkName ?? throw new ArgumentNullException(frameworkName);
        }

        /// <summary>
        /// Gets the name of the .NET version against which a particular assembly was compiled.
        /// </summary>
        // The target framework moniker that this assembly was compiled against.
        // Use the FrameworkName class to interpret target framework monikers.
        public string FrameworkName => _frameworkName;

        /// <summary>
        /// Gets the display name of the .NET version against which an assembly was built.
        /// </summary>
        public string? FrameworkDisplayName
        {
            get => _frameworkDisplayName;
            set => _frameworkDisplayName = value;
        }
    }
}
#endif