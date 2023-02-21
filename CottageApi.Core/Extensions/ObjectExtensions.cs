/*using System.Collections.Generic;

namespace CottageApi.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T FormToObject<T>(this IFormCollection<string, Microsoft.Extensions.Primitives.StringValues> source)
        where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in source)
            {
                someObjectType
                         .GetProperty(item.Key)
                         .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }
    }
}*/
