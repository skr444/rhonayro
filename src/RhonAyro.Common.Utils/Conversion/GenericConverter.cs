using System;
using System.Collections.Concurrent;

namespace RhonAyro.Common.Utils.Conversion
{
    /// <summary>
    /// Provides tools for generic type conversions.
    /// </summary>
    public static class GenericConverter
    {
        private delegate bool TryParseObj(string s, out object? value);
        public delegate bool TryParse<T>(string s, out T value);

        private static readonly ConcurrentDictionary<Type, TryParseObj> Parsers = new();

        static GenericConverter()
        {
            // Register common primitives and BCL structs
            Register<int>(static (string s, out int v) => int.TryParse(s, out v));
            Register<long>(static (string s, out long v) => long.TryParse(s, out v));
            Register<double>(static (string s, out double v) => double.TryParse(s, out v));
            Register<decimal>(static (string s, out decimal v) => decimal.TryParse(s, out v));
            Register<bool>(static (string s, out bool v) => bool.TryParse(s, out v));
            Register<Guid>(static (string s, out Guid v) => Guid.TryParse(s, out v));
            Register<DateTime>(static (string s, out DateTime v) => DateTime.TryParse(s, out v));
            Register<TimeSpan>(static (string s, out TimeSpan v) => TimeSpan.TryParse(s, out v));

            // Strings are "always convertible"
            Parsers[typeof(string)] = static (string s, out object? v) => { v = s; return true; };
        }

        public static void Register<T>(TryParse<T> tryParse)
        {
            Parsers[typeof(T)] = Wrap(tryParse);
        }

        public static bool TryConvert<T>(string? value, out T? result)
        {
            result = default;

            var t = typeof(T);
            var underlying = Nullable.GetUnderlyingType(t);
            var target = underlying ?? t;

            // Empty/whitespace -> null for Nullable<T>
            if (string.IsNullOrWhiteSpace(value))
            {
                if (underlying is not null)
                {
                    return true; // result stays default(null)
                }

                return false;
            }

            // Enums (cached per enum type)
            if (target.IsEnum)
            {
                var enumParser = Parsers.GetOrAdd(target, CreateEnumParser(target));
                if (enumParser(value!, out var boxed))
                {
                    // boxed is an instance of the enum type -> cast through object
                    result = (T)(object)boxed!;
                    return true;
                }

                return false;
            }

            // Regular registered types
            if (Parsers.TryGetValue(target, out var parser) && parser(value!, out var obj))
            {
                // Bridge generic boundary once. For Nullable<T>, this remains correct.
                result = (T)(object)obj!;
                return true;
            }

            return false;
        }

        private static TryParseObj Wrap<T>(TryParse<T> inner)
        {
            // Box only on success; avoids double-casting in call-sites.
            return (string s, out object? boxed) =>
            {
                if (inner(s, out var v))
                {
                    boxed = v!;
                    return true;
                }

                boxed = null;
                return false;
            };
        }

        private static TryParseObj CreateEnumParser(Type enumType)
        {
            // Build a closed generic method for Enum.TryParse<TEnum>(string, bool, out TEnum)
            var method = typeof(GenericConverter)
                .GetMethod(nameof(ParseEnumGeneric),
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .MakeGenericMethod(enumType);

            return (TryParseObj)Delegate.CreateDelegate(typeof(TryParseObj), method);
        }

        private static bool ParseEnumGeneric<TEnum>(string s, out object? boxed) where TEnum : struct, Enum
        {
            // Ignore case is typically friendlier; adjust if you want strict matches.
            if (Enum.TryParse<TEnum>(s, ignoreCase: true, out var e))
            {
                boxed = e;
                return true;
            }

            boxed = null;
            return false;
        }
    }
}
