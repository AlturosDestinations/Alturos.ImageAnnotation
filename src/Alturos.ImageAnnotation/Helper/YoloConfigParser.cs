using Alturos.ImageAnnotation.Model.YoloConfig;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

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
                if (line.Length == 0)
                {
                    continue;
                }

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
                        var splitString = line.Replace(" = ", "=").Split('=');

                        var propertyName = SnakeCaseToPascalCase(splitString[0]);
                        var value = splitString[1];

                        var obj = yoloConfig.YoloConfigElements.Last();
                        var property = obj.GetType().GetProperty(propertyName);

                        if (property.PropertyType == typeof(int))
                        {
                            if (int.TryParse(value, out var intValue))
                            {
                                property.SetValue(obj, intValue);
                            }
                        }
                        else if (property.PropertyType == typeof(float))
                        {
                            property.SetValue(obj, float.Parse(value, CultureInfo.InvariantCulture));
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
                            var values = value.Split(new string[] { ",  " }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.Split(',').Select(p => int.Parse(p)).ToArray()).ToArray();
                            property.SetValue(obj, values);
                        }
                        else if (property.PropertyType == typeof(float[]))
                        {
                            var values = value.Split(',').Select(o => float.Parse(o, CultureInfo.InvariantCulture)).ToArray();
                            property.SetValue(obj, values);
                        }

                        break;
                }
            }

            return yoloConfig;
        }

        public static string Compose(YoloConfig yoloConfig)
        {
            var sb = new StringBuilder();

            var cachedCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            foreach (var element in yoloConfig.YoloConfigElements)
            {
                var type = element.GetType();

                if (sb.Length > 0) {
                    sb.AppendLine();
                }
                sb.AppendLine($"[{PascalCaseToSnakeCase(type.Name)}]");

                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var propertyType = property.PropertyType;
                    var value = property.GetValue(element);
                    var valueStr = string.Empty;

                    if (propertyType == typeof(int[]))
                    {
                        var intValues = value as int[];
                        valueStr = string.Concat(intValues.Select((o, i) => i < intValues.Length - 1 ? o.ToString() + "," : o.ToString()));
                    }
                    else if (propertyType == typeof(int[][]))
                    {
                        var intValues = value as int[][];
                        for (var i = 0; i < intValues.Length; i++)
                        {
                            valueStr += string.Concat(intValues[i].Select((o, j) => j < intValues[i].Length - 1 ? o.ToString() + "," : o.ToString()));
                            if (i != intValues.Length - 1)
                            {
                                valueStr += ",  ";
                            }
                        }
                    }
                    else if (propertyType == typeof(float[]))
                    {
                        var floatValues = value as float[];
                        valueStr = string.Concat(floatValues.Select((o, i) => i < floatValues.Length - 1 ? o.ToString() + "," : o.ToString()));
                    }
                    else
                    {
                        valueStr = value.ToString();
                    }

                    sb.AppendLine($"{PascalCaseToSnakeCase(property.Name)}={PascalCaseToSnakeCase(valueStr)}");
                }
            }

            CultureInfo.CurrentCulture = cachedCulture;

            return sb.ToString();
        }

        private static string SnakeCaseToPascalCase(string snakeCase)
        {
            return snakeCase.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        public static string PascalCaseToSnakeCase(string pascalCase)
        {
            return string.Concat(pascalCase.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
