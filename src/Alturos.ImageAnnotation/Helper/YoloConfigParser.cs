using Alturos.ImageAnnotation.Model.YoloConfig;
using System;
using System.IO;
using System.Linq;

namespace Alturos.ImageAnnotation.Helper
{
    public static class YoloConfigParser
    {
        public static YoloConfig Parse(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var yoloConfig = new YoloConfig();

            foreach (var line in lines)
            {
                var firstLetter = line[0];
                switch (firstLetter)
                {
                    case '[':
                        var lineWithoutBrackets = line.Replace("[", "").Replace("]", "");
                        var className = SnakeCaseToPascalCase(lineWithoutBrackets);

                        var type = Type.GetType($"Alturos.ImageAnnotation.Model.YoloConfig.{className}");
                        var item = Activator.CreateInstance(type) as YoloConfigElement;
                        yoloConfig.YoloConfigElements.Add(item);

                        break;

                    case '#':
                        break;

                    default:
                        var splitString = line.Split('=');

                        var propertyName = SnakeCaseToPascalCase(splitString[0]);
                        var value = splitString[1];

                        var obj = yoloConfig.YoloConfigElements.Last();
                        var property = obj.GetType().GetProperty(propertyName);

                        if (property.PropertyType == typeof(int))
                        {
                            property.SetValue(obj, int.Parse(value));
                        }
                        else if (property.PropertyType == typeof(float))
                        {
                            property.SetValue(obj, float.Parse(value));
                        }
                        else if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(obj, value);
                        }
                        else if (property.PropertyType == typeof(int[]))
                        {
                            var values = value.Split(',').Select(o => int.Parse(o)).ToArray();
                            property.SetValue(obj, values);
                        }
                        else if (property.PropertyType == typeof(int[][]))
                        {
                            var values = value.Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.Split(',').Select(p => int.Parse(p)).ToArray()).ToArray();
                            property.SetValue(obj, values);
                        }

                        break;
                }
            }

            return yoloConfig;
        }

        private static string SnakeCaseToPascalCase(string snakeCase)
        {
            return snakeCase.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }
}
