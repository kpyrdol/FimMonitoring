using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FIMMonitoring.Common
{
    public static class Extension
    {
        public static string GetEnumName(this Enum value)
        {
            if (value == null) return string.Empty;

            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name == null) return "";
            var field = type.GetField(name);

            if (field == null) return "";
            var atr = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;

            return atr != null ? atr.Name : "";
        }

        public static string GetEnumDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name == null) return "";
            var field = type.GetField(name);

            if (field == null) return "";
            var atr = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;

            return atr != null ? atr.Description : "";
        }

        public static Enum GetEnumByDisplayName(this Type type, string name)
        {

            var fields = type.GetFields();
            var e =
                from field in fields
                let b = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute
                where b != null && (string.Equals(b.Name, name, StringComparison.InvariantCultureIgnoreCase))
                select (Enum)Enum.Parse(type, field.Name);

            if (!e.Any())
            {
                int enumIntValue = 0;
                int.TryParse(name, out enumIntValue);
                return (Enum)Enum.ToObject(type, enumIntValue);
            }
            return e.FirstOrDefault();
        }
    }
}
