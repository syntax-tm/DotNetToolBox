namespace DotNetToolBox.Extensions
{
    public static class ReflectionExtensions
    {

        public static string GetClassName(this object source)
        {
            var sourceType = source.GetType();
            if (sourceType.BaseType == null)
            {
                return sourceType.Name;
            }

            if (sourceType.BaseType.IsAbstract)
            {
                return sourceType.Name;
            }

            if (sourceType.BaseType.IsInterface)
            {
                return sourceType.Name;
            }

            var currentType = sourceType;
            while (currentType.BaseType != null)
            {
                if (currentType.BaseType.IsAbstract)
                {
                    return currentType.Name;
                }

                var currentNamespace = currentType.BaseType.Namespace;

                if (!string.IsNullOrEmpty(currentNamespace) && 
                    (currentNamespace.StartsWith("System.Data") || currentNamespace.StartsWith("DevExpress")))
                {
                    return currentType.Name;
                }
                currentType = currentType.BaseType;
            }
            return currentType.Name;
        }

    }
}
