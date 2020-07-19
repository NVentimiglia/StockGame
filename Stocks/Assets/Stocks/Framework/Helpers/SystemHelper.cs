using System;

namespace Framework
{
    public static class SystemHelper
    {
        public static T SafeDispose<T>(this T instance) where T : IDisposable
        {
            var obj = instance;
            if (obj != null)
            {
                obj.Dispose();
            }
            return default(T);
        }

        public static T SafeInvoke<T>(this Func<T> callback)
        {
            var cb = callback;
            if (cb != null)
            {
                return cb();
            }
            return default(T);
        }

        public static void SafeInvoke<T>(this Action<T> callback, T value)
        {
            var cb = callback;
            if (cb != null)
            {
                cb(value);
            }
        }

        public static void SafeInvoke(this Action callback)
        {
            var cb = callback;
            if (cb != null)
            {
                cb();
            }
        }
    }
}