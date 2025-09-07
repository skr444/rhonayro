using System;
using System.Collections.Generic;
using System.Linq;

namespace RhonAyro.Common.Data.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Assembles a string from a sequence of objects with the specified transformer.
        /// </summary>
        /// <typeparam name="T">The type of the items in the sequence.</typeparam>
        /// <param name="source">The source sequence to convert into a string.</param>
        /// <param name="transformer">The transformer function that returns the string representation of each item.</param>
        /// <param name="separator">A sequence of separator characters.</param>
        /// <returns>A custom string representation of a sequence of objects.</returns>
        public static string ToCustomString<T>(this IEnumerable<T> source, Func<T, string> transformer = null!, string separator = ", ")
        {
            if ((source == null) || !source.Any())
            {
                return String.Empty;
            }

            if (transformer == null)
            {
                return String.Join(separator, source.Select(x => x?.ToString() ?? String.Empty));
            }

            return String.Join(separator, source.Select(transformer));
        }
    }
}
