using System;
using Newtonsoft.Json;

namespace DotNetToolBox.Extensions
{
    public static class ObjectExtensions
    {


        /// <summary>
        /// Clones the <typeparamref name="T"/> object to a new instance by serializing and
        /// then de-serializing the objects properties with <see cref="JsonConvert"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of object being cloned.</typeparam>
        /// <param name="source">The <typeparamref name="T"/> object instance being cloned.</param>
        /// <returns>A new <typeparamref name="T"/> instance containing the <see cref="source"/> object's information.</returns>
        public static T Clone<T>(this T source)
            where T : class
        {
            if (source == null) return null;
            var sourceSerialized = JsonConvert.SerializeObject(source, Formatting.None);
            var clonedObject = JsonConvert.DeserializeObject<T>(sourceSerialized);
            return clonedObject;
        }

    }
}
