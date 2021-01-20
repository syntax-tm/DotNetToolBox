using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using log4net;

namespace DotNetToolBox.Extensions
{
    public static class AttributeExtensions
    {

        private static readonly ILog log = LogManager.GetLogger(nameof(AttributeExtensions));

        /// <summary>
        /// Gets the <c>Description</c> attribute for the source <see cref="Enum"/>.
        /// </summary>
        /// <seealso cref="DescriptionAttribute"/>
        /// <seealso cref="DisplayAttribute"/>
        /// <seealso cref="DisplayNameAttribute"/>
        /// <seealso cref="DXDisplayNameAttribute"/>
        public static string GetDescription(this Enum source)
        {
            var fi = source.GetType().GetField(source.ToString());
            try
            {
                //  check for the [Description] attribute on the field
                var description = fi.GetCustomAttribute<DescriptionAttribute>();
                if (description != null)
                {
                    return description.Description;
                }

                //  check for the [Display] attribute on the field
                var display = fi.GetCustomAttribute<DisplayAttribute>();
                if (display != null)
                {
                    if (!string.IsNullOrEmpty(display.Description))
                    {
                        return display.Description;
                    }
                    return display.Name;
                }

                //  check for the [DisplayName] attribute on the field
                var displayName = fi.GetCustomAttribute<DisplayNameAttribute>();
                if (displayName != null)
                {
                    return displayName.DisplayName;
                }
            }
            catch (Exception e)
            {
                log.Error($"An error occurred getting the description of {nameof(source)} {fi.Name}. {e.Message}", e);
            }

            return fi.Name;
        }

        /// <summary>
        /// Returns the <c>Name</c> property of the <see cref="DisplayAttribute"/> for the
        /// given <see cref="source"/> <see cref="Enum"/>.
        /// </summary>
        /// <seealso cref="DisplayAttribute"/>
        /// <seealso cref="DisplayNameAttribute"/>
        public static string GetName(this Enum source, bool useShort = false)
        {
            try
            {
                var fi = source.GetType().GetField(source.ToString());
                try
                {
                    //  check for the [Display] attribute on the field
                    var displayAttributes = fi.GetCustomAttribute<DisplayAttribute>();
                    if (displayAttributes != null)
                    {
                        if (!string.IsNullOrEmpty(displayAttributes.ShortName) && useShort)
                        {
                            return displayAttributes.ShortName;
                        }
                        return displayAttributes.Name;
                    }

                    //  check for the [DisplayName] attribute on the field
                    var displayNameAttribute = fi.GetCustomAttribute<DisplayNameAttribute>();
                    if (displayNameAttribute != null)
                    {
                        return displayNameAttribute.DisplayName;
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Error getting the name attribute of {fi.Name}. {e.Message}", e);
                }

                return fi.Name;
            }
            catch (Exception)
            {
                return source?.ToString() ?? string.Empty;
            }
        }


        /// <summary>
        /// Returns the <c>Name</c> property of the <see cref="DisplayAttribute"/> for the
        /// given <see cref="source"/> <see cref="object"/>.
        /// </summary>
        /// <seealso cref="DisplayAttribute"/>
        /// <seealso cref="DisplayNameAttribute"/>
        public static string GetName(this object source, bool useShort = false)
        {
            try
            {
                var fi = source.GetType().GetField(source.ToString());
                try
                {
                    //  check for the [Display] attribute on the field
                    var displayAttributes = fi.GetCustomAttribute<DisplayAttribute>();
                    if (displayAttributes != null)
                    {
                        if (!string.IsNullOrEmpty(displayAttributes.ShortName) && useShort)
                        {
                            return displayAttributes.ShortName;
                        }
                        return displayAttributes.Name;
                    }

                    //  check for the [DisplayName] attribute on the field
                    var displayNameAttribute = fi.GetCustomAttribute<DisplayNameAttribute>();
                    if (displayNameAttribute != null)
                    {
                        return displayNameAttribute.DisplayName;
                    }
                }
                catch (Exception e)
                {
                    log.Error($"Error getting the name attribute of {fi.Name}. {e.Message}", e);
                }

                return fi.Name;
            }
            catch (Exception)
            {
                return source?.ToString() ?? string.Empty;
            }
        }

    }
}
