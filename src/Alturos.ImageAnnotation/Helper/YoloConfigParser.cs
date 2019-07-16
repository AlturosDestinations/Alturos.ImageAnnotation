using Alturos.ImageAnnotation.Model.YoloConfig;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Alturos.ImageAnnotation.Helper
{
    public static class YoloConfigParser
    {
        #region Parsing (text file to Yolo config model)

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

                        try
                        {
                            var parsedProperty = ParseProperty(property.PropertyType, value);
                            property.SetValue(obj, parsedProperty);
                        }
                        catch (Exception ex)
                        {
                        }

                        break;
                }
            }

            return yoloConfig;
        }

        private static object ParseProperty(Type type, string value)
        {
            object parsedProperty = null;

            var arrayRank = type.IsArray ? type.GetArrayRank() : 0;
            var elementType = type.IsArray ? type.GetElementType() : type;

            switch (arrayRank)
            {
                case 0:
                    parsedProperty = ParseValue(elementType, value);
                    break;
                case 1:
                    parsedProperty = ParseArray(elementType, value);
                    break;
                case 2:
                    parsedProperty = Parse2DArray(elementType, value);
                    break;
            }

            return parsedProperty;
        }

        private static object ParseValue(Type elementType, string value)
        {
            if (elementType == typeof(int))
            {
                if (int.TryParse(value, out var intValue))
                {
                    return intValue;
                }
            }
            else if (elementType == typeof(float))
            {
                return float.Parse(value, CultureInfo.InvariantCulture);
            }
            else if (elementType == typeof(string))
            {
                return value;
            }

            return null;
        }

        private static object[] ParseArray(Type elementType, string value)
        {
            var parsedValues = value.Split(',').Select(o => ParseValue(elementType, o)).ToArray();
            return parsedValues;
        }

        private static object[,] Parse2DArray(Type elementType, string value)
        {
            var parsedValues = value.Split(new string[] { ",  " }, StringSplitOptions.RemoveEmptyEntries);
            return parsedValues.Select(o => ParseArray(elementType, o)).ToArray().To2DArray();
        }

        #endregion

        #region Composing (Yolo config model to text file)

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

                    var composedProperty = ComposeProperty(propertyType, value);

                    sb.AppendLine($"{PascalCaseToSnakeCase(property.Name)}={PascalCaseToSnakeCase(composedProperty)}");
                }
            }

            CultureInfo.CurrentCulture = cachedCulture;

            return sb.ToString();
        }

        private static string ComposeProperty(Type type, object value)
        {
            var composedProperty = string.Empty;

            var arrayRank = type.IsArray ? type.GetArrayRank() : 0;
            switch (arrayRank)
            {
                case 0:
                    composedProperty = ComposeValue(value);
                    break;
                case 1:
                    var values1d = value as object[];
                    composedProperty = ComposeArray(values1d);
                    break;
                case 2:
                    var values2d = value as object[,];
                    composedProperty = Compose2DArray(values2d);
                    break;
            }

            return composedProperty;
        }

        private static string ComposeValue(object value)
        {
            return value.ToString();
        }

        private static string ComposeArray(object[] values)
        {
            return string.Concat(values.Select((o, i) => i < values.Length - 1 ? ComposeValue(o) + "," : ComposeValue(o)));
        }

        private static string Compose2DArray(object[,] values)
        {
            var composedValue = string.Empty;

            for (var i = 0; i < values.Length; i++)
            {
                composedValue += ComposeArray(new object[] { values[i, 0], values[i, 1] });
                if (i != values.Length - 1)
                {
                    composedValue += ",  ";
                }
            }

            return composedValue;
        }

        #endregion

        #region Case converters

        private static string SnakeCaseToPascalCase(string snakeCase)
        {
            return snakeCase.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1)).Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }

        public static string PascalCaseToSnakeCase(string pascalCase)
        {
            return string.Concat(pascalCase.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        #endregion
    }
}
