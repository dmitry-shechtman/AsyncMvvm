using System.Reflection;

namespace Ditto.AsyncMvvm
{
    internal static class PropertyInfoExtensions
    {
        //[Property]
        public static MethodInfo GetGetMethod(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod;
        }
    }
}
