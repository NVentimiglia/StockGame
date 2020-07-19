using System;
using System.Collections.Generic;
using System.Reflection;

namespace Framework
{
    public static class ReflectionHelper
    {
        static readonly Dictionary<Type, FieldInfo[]> _fieldCache = new Dictionary<Type, FieldInfo[]>();
        static readonly Dictionary<Type, PropertyInfo[]> _propertyCache = new Dictionary<Type, PropertyInfo[]>();
        static readonly Dictionary<MemberInfo, Attribute> _attrCache = new Dictionary<MemberInfo, Attribute>();

        public static FieldInfo[] GetFields(Type t)
        {
            FieldInfo[] fields;
            if (!_fieldCache.TryGetValue(t, out fields))
            {
                fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                _fieldCache.Add(t, fields);
            }
            return fields;
        }

        public static PropertyInfo[] GetProperties(Type t)
        {
            PropertyInfo[] fields;
            if (!_propertyCache.TryGetValue(t, out fields))
            {
                fields = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                _propertyCache.Add(t, fields);
            }
            return fields;
        }

        public static T GetAttribute<T>(this Type prop) where T : Attribute
        {
            Attribute attr = null;
            if (!_attrCache.TryGetValue(prop, out attr))
            {
                var attrs = prop.GetCustomAttributes(typeof(T), true);
                if (attrs != null && attrs.Length > 0)
                {
                    attr = (T)attrs[0];
                    _attrCache.Add(prop, attr);
                }
            }
            return attr as T;
        }

        public static T GetAttribute<T>(this FieldInfo prop) where T : Attribute
        {
            Attribute attr = null;
            if (!_attrCache.TryGetValue(prop, out attr))
            {
                var attrs = prop.GetCustomAttributes(typeof(T), true);
                if (attrs != null && attrs.Length > 0)
                {
                    attr = (T)attrs[0];
                    _attrCache.Add(prop, attr);
                }
            }
            return attr as T;
        }

        public static T GetAttribute<T>(this PropertyInfo prop) where T : Attribute
        {
            Attribute attr = null;
            if (!_attrCache.TryGetValue(prop, out attr))
            {
                var attrs = prop.GetCustomAttributes(typeof(T), true);
                if (attrs != null && attrs.Length > 0)
                {
                    attr = (T)attrs[0];
                    _attrCache.Add(prop, attr);
                }
            }
            return attr as T;
        }

        public static T GetAttribute<T>(this Type t, bool inherit) where T : Attribute
        {
            var a = t.GetCustomAttributes(typeof(T), inherit);
            if (a == null || a.Length == 0)
                return null;
            return a[0] as T;
        }
    }
}