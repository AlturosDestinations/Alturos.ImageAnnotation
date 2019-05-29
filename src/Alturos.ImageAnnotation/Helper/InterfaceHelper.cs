using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Alturos.ImageAnnotation.Helper
{
    public static class InterfaceHelper
    {
        public static List<NameValueObject> GetImplementations<T>()
        {
            var type = typeof(T);

            if (!type.IsInterface)
            {
                throw new NotSupportedException();
            }

            var classes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass).ToList();

            var objects = classes
            .Select(value => new NameValueObject
            {
                Name = (value.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute).Description,
                Value = value
            })
            .OrderBy(item => item.Value.ToString())
            .ToList();

            return objects;
        }
    }
}
