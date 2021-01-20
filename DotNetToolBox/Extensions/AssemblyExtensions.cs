using System;
using System.Reflection;

namespace DotNetToolBox.Extensions
{
    public static class AssemblyExtensions
    {

        public static T GetAssemblyAttribute<T>(this Assembly assembly) where T : Attribute
        {
            try
            {
                var attributes = assembly.GetCustomAttributes(typeof(T), true);
                if (attributes.Length == 0) return null;
                return (T) attributes[0];
            }
            catch
            {
                return null;
            }
        }

    }
}
