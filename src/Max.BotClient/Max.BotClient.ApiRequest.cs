using System;
using System.Collections.Generic;
using System.Reflection;

namespace Max.BotClient
{
    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class QueryParamAttribute : Attribute
    {
        public string? Name { get; }
        public QueryParamAttribute(string? name = null) => Name = name;
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class BodyParamAttribute : Attribute
    {
        public string? Name { get; }
        public BodyParamAttribute(string? name = null) => Name = name;
    }

    [AttributeUsage(AttributeTargets.Property)]
    internal sealed class BodyAttribute : Attribute { }

    internal static class ApiRequestBinder
    {
        public static void Bind<T>(T request, string basePath, out string path, out object? body)
            where T : class
        {
            var queryParts = new List<string>();
            var bodyDict = new Dictionary<string, object?>();
            object? directBody = null;
            var hasBodyProp = false;

            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(request);
                if (value == null)
                    continue;

                var queryAttr = prop.GetCustomAttribute<QueryParamAttribute>();
                if (queryAttr != null)
                {
                    var str = ConvertToQueryString(value);
                    if (str != null)
                    {
                        var name = queryAttr.Name ?? prop.Name.ToSnakeCase();
                        queryParts.Add($"{name}={str}");
                    }
                    continue;
                }

                var bodyParamAttr = prop.GetCustomAttribute<BodyParamAttribute>();
                if (bodyParamAttr != null)
                {
                    var name = bodyParamAttr.Name ?? prop.Name.ToSnakeCase();
                    bodyDict[name] = value;
                    continue;
                }

                if (prop.GetCustomAttribute<BodyAttribute>() != null)
                {
                    directBody = value;
                    hasBodyProp = true;
                }
            }

            path = queryParts.Count > 0
                ? basePath + "?" + string.Join("&", queryParts)
                : basePath;

            if (hasBodyProp)
                body = directBody;
            else if (bodyDict.Count > 0)
                body = bodyDict;
            else
                body = null;
        }

        private static string? ConvertToQueryString(object value)
        {
            var type = value.GetType();

            if (type.IsEnum)
                return value.ToString()!.ToSnakeCase();

            if (type == typeof(bool))
                return ((bool)value).ToString().ToLowerInvariant();

            if (type.IsArray)
            {
                var elementType = type.GetElementType()!;
                var arr = (Array)value;
                if (arr.Length == 0)
                    return null;

                var parts = new List<string>(arr.Length);
                foreach (var elem in arr)
                {
                    if (elem == null)
                        continue;
                    if (elementType.IsEnum)
                        parts.Add(elem.ToString()!.ToSnakeCase());
                    else if (elementType == typeof(string))
                        parts.Add(Uri.EscapeDataString((string)elem));
                    else
                        parts.Add(elem.ToString()!);
                }

                return parts.Count > 0 ? string.Join(",", parts) : null;
            }

            if (type == typeof(string))
                return Uri.EscapeDataString((string)value);

            return value.ToString();
        }
    }
}
