using Alturos.ImageAnnotation.Model.YoloConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

                        var parsedProperty = ParseProperty(property.PropertyType, value);
                        property.SetValue(obj, parsedProperty);

                        break;
                }
            }

            return yoloConfig;
        }

        private static object ParseProperty(Type type, string value)
        {
            var arrayRank = type.IsArray ? type.GetArrayRank() : 0;
            var elementType = type.IsArray ? type.GetElementType() : type;

            var methodToCall = string.Empty;

            switch (arrayRank)
            {
                case 0:
                    methodToCall = nameof(ParseValue);
                    break;
                case 1:
                    methodToCall = nameof(ParseArray);
                    break;
                case 2:
                    methodToCall = nameof(Parse2DArray);
                    break;
            }

            var methodInfo = typeof(YoloConfigParser).GetMethod(methodToCall, BindingFlags.NonPublic | BindingFlags.Static);
            var genericMethod = methodInfo.MakeGenericMethod(elementType);
            var parsedProperty = genericMethod.Invoke(null, new object[] { value });

            return parsedProperty;
        }

        private static T ParseValue<T>(string value)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    return (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
                }
                return default(T);
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }

        private static T[] ParseArray<T>(string value)
        {
            var parsedValues = value.Split(',').Select(o => ParseValue<T>(o)).ToArray();
            return parsedValues;
        }

        private static T[,] Parse2DArray<T>(string value)
        {
            var parsedValues = value.Split(new string[] { ",  " }, StringSplitOptions.RemoveEmptyEntries);
            return parsedValues.Select(o => ParseArray<T>(o)).ToArray().To2DArray();
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
            var arrayRank = type.IsArray ? type.GetArrayRank() : 0;
            var elementType = type.IsArray ? type.GetElementType() : type;

            var methodToCall = string.Empty;

            switch (arrayRank)
            {
                case 0:
                    methodToCall = nameof(ComposeValue);
                    break;
                case 1:
                    methodToCall = nameof(ComposeArray);
                    break;
                case 2:
                    methodToCall = nameof(Compose2DArray);
                    break;
            }

            var methodInfo = typeof(YoloConfigParser).GetMethod(methodToCall, BindingFlags.NonPublic | BindingFlags.Static);
            var genericMethod = methodInfo.MakeGenericMethod(elementType);
            var composedProperty = genericMethod.Invoke(null, new object[] { value });

            return composedProperty.ToString();
        }

        private static string ComposeValue<T>(T value)
        {
            return value.ToString();
        }

        private static string ComposeArray<T>(T[] values)
        {
            return string.Concat(values.Select((o, i) => i < values.Length - 1 ? ComposeValue(o) + "," : ComposeValue(o)));
        }

        private static string Compose2DArray<T>(T[,] values)
        {
            var composedValue = string.Empty;
            var length = values.GetLength(0);

            for (var i = 0; i < length; i++)
            {
                composedValue += ComposeArray(new object[] { values[i, 0], values[i, 1] });
                if (i != length - 1)
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
