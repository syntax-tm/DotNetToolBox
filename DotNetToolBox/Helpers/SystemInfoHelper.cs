using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using log4net;
using Microsoft.Win32;
using Environment = System.Environment;

namespace DotNetToolBox.Helpers
{
    public static class SystemInfoHelper
    {
        private const string _windowsVersionRegPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
        private const string _dotNetVersionRegPath = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

        private const int _maxOsVersionLength = 100;

        private static readonly ILog log = LogManager.GetLogger(nameof(SystemInfoHelper));

        private static string _userName;
        private static string _userDomainName;
        private static string _machineName;
        private static string _dotNetVersion;
        private static string _productName;
        private static string _releaseId;
        private static string _productId;
        private static string _editionID;
        private static string _currentBuildNumber;
        private static WindowsIdentity _currentUserIdentity;
        private static WindowsPrincipal _currentUserPrincipal;

        /// <summary>
        /// The user name of the currently logged in user.
        /// </summary>
        public static string UserName
        {
            get
            {
                if (_userName != null) return _userName;
                _userName = Environment.UserName;
                return _userName;
            }
        }

        /// <summary>
        /// The user's domain name of the currently logged in user.
        /// </summary>
        public static string UserDomainName
        {
            get
            {
                if (_userDomainName != null) return _userDomainName;
                _userDomainName = Environment.UserDomainName;
                return _userDomainName;
            }
        }

        /// <summary>
        /// Returns the <see cref="UserName"/> and the <see cref="UserDomainName"/> in 'DOMAIN\USERNAME' format.
        /// </summary>
        public static string UserNameWithDomain = $@"{UserDomainName}\{UserName}";

        /// <summary>
        /// The <see cref="WindowsIdentity" /> of the currently logged in user.
        /// </summary>
        /// <seealso cref="WindowsIdentity.GetCurrent()" />
        public static WindowsIdentity CurrentUserIdentity
        {
            get
            {
                if (_currentUserIdentity != null) return _currentUserIdentity;
                _currentUserIdentity = WindowsIdentity.GetCurrent();
                return _currentUserIdentity;
            }
        }

        /// <summary>
        /// The <see cref="WindowsPrincipal"/> for the currently logged in user.
        /// </summary>
        public static WindowsPrincipal CurrentUserPrincipal
        {
            get
            {
                if (_currentUserPrincipal != null) return _currentUserPrincipal;
                _currentUserPrincipal = new WindowsPrincipal(CurrentUserIdentity);
                return _currentUserPrincipal;
            } 
        }

        /// <summary>
        /// Checks the current user's <see cref="WindowsPrincipal" /> for the built-in
        /// <see cref="WindowsBuiltInRole.Administrator" /> role.
        /// </summary>
        /// <seealso cref="WindowsPrincipal.IsInRole(WindowsBuiltInRole)" />
        public static bool IsWindowsAdmin
        {
            get => CurrentUserPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// The name of the machine currently running.
        /// </summary>
        public static string MachineName
        {
            get
            {
                if (_machineName != null) return _machineName;
                _machineName = Environment.MachineName;
                return _machineName;
            }
        }

        /// <summary>
        /// Returns the IP address of the first <see cref="AddressFamily.InterNetwork" /> DNS entry found
        /// for this machine, or 0.0.0.0 if none are found.
        /// </summary>
        public static string IPAddress
        {
            get
            {
                var ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                var ipAddressOfMachine = ipHostEntry.AddressList.FirstOrDefault(addressListItem => addressListItem.AddressFamily == AddressFamily.InterNetwork);
                if (null != ipAddressOfMachine)
                {
                    return ipAddressOfMachine.ToString();
                }
                return "0.0.0.0";
            }
        }

        /// <summary>
        /// Returns the OS version information for the currently machine, with a max length
        /// of 100 characters by default.
        /// </summary>
        public static string OSVersion
        {
            get
            {
                var platform = Environment.OSVersion.Platform;

                if (platform == PlatformID.MacOSX || platform == PlatformID.Xbox || platform == PlatformID.Unix)
                {
                    var version = Environment.OSVersion.VersionString;
                    if (version.Length > _maxOsVersionLength)
                    {
                        version = version.Substring(0, _maxOsVersionLength);
                    }
                    return version;
                }

                //  Windows platform
                return $"{WindowsProductName} {WindowsReleaseID}";
            }
        }

        /// <summary>
        /// Returns the simple version name for the currently installed .NET framework version.
        /// </summary>
        public static string DotNetVersion
        {
            get
            {
                if (_dotNetVersion != null) return _dotNetVersion;
                using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(_dotNetVersionRegPath))
                {
                    var releaseKey = ndpKey?.GetValue("Release");
                    if (releaseKey != null)
                    {
                        _dotNetVersion = GetDotNetVersionFromDWord((int) releaseKey);
                        return _dotNetVersion;
                    }
                    log.Error(".NET Framework Version 4.5 or later is not detected.");
                    _dotNetVersion = string.Empty;
                }
                return _dotNetVersion;
            }
        }

        /// <summary>
        /// Returns the <see cref="Registry"/> value indicating the current Windows release.
        /// </summary>
        public static string WindowsReleaseID
        {
            get
            {
                if (_releaseId != null) return _releaseId;
                _releaseId = Registry.GetValue(_windowsVersionRegPath, @"ReleaseID", string.Empty).ToString();
                return _releaseId;
            }
        }

        /// <summary>
        /// Returns the <see cref="Registry"/> value indicating the current Windows product.
        /// </summary>
        public static string WindowsProductID
        {
            get
            {
                if (_productId != null) return _productId;
                _productId = Registry.GetValue(_windowsVersionRegPath, @"ProductID", string.Empty).ToString();
                return _productId;
            }
        }

        /// <summary>
        /// Returns the <see cref="Registry"/> value indicating the current Windows edition.
        /// </summary>
        public static string WindowsEditionID
        {
            get
            {
                if (_editionID != null) return _editionID;
                _editionID = Registry.GetValue(_windowsVersionRegPath, @"EditionID", string.Empty).ToString();
                return _editionID;
            }
        }
        
        /// <summary>
        /// Returns the <see cref="Registry"/> value indicating the current Windows product name.
        /// </summary>
        public static string WindowsProductName
        {
            get
            {
                if (_productName != null) return _productName;
                _productName = Registry.GetValue(_windowsVersionRegPath, @"ProductName", string.Empty).ToString();
                return _productName;
            }
        }

        /// <summary>
        /// Returns the <see cref="Registry"/> value indicating the current Windows release.
        /// </summary>
        public static string WindowsBuildNumber
        {
            get
            {
                if (_currentBuildNumber != null) return _currentBuildNumber;
                _currentBuildNumber = Registry.GetValue(_windowsVersionRegPath, @"CurrentBuildNumber", string.Empty).ToString();
                return _currentBuildNumber;
            }
        }

        /// <summary>
        /// Converts the <c>DWORD</c> release key value stored in the registry to the name of
        /// the associated .NET framework install.
        /// </summary>
        /// <param name="releaseKey">The <c>DWORD</c> registry value found in 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\'.</param>
        private static string GetDotNetVersionFromDWord(int releaseKey)
        {
            if (releaseKey >= 461808) return "4.7.2 or later";
            if (releaseKey >= 461308) return "4.7.1";
            if (releaseKey >= 460798) return "4.7";
            if (releaseKey >= 394802) return "4.6.2";
            if (releaseKey >= 394254) return "4.6.1";
            if (releaseKey >= 393295) return "4.6";
            if (releaseKey >= 379893) return "4.5.2";
            if (releaseKey >= 378675) return "4.5.1";
            if (releaseKey >= 378389) return "4.5";
            // This code should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            log.Error($"No 4.5 or later version detected. Release key {releaseKey} DWord not valid.");
            return null;
        }

    }
}
