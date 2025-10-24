using System;
using System.Collections.Generic;

using RhonAyro.Common.Utils.Conversion;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Infrastructure.Storage.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IViewStateRepository"/>.
    /// </summary>
    public static class ViewStateRepositoryExtensions
    {
        /// <summary>
        /// Gets a value of the specified type.
        /// </summary>
        /// <typeparam name="T">Expected type of the value.</typeparam>
        /// <param name="source">The repository to query.</param>
        /// <param name="key">The key associated with the value that should be retrieved.</param>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="key"/> is <see langword="null"/> or empty.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        ///     If no entry was found for the specified <paramref name="key"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     If <paramref name="source"/> is <see langword="null"/>.
        ///     If the value is not of the expected type <typeparamref name="T"/>.
        /// </exception>
        public static T GetAs<T>(this IViewStateRepository source, string key)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

            if (!source.TryGet(key, out string? value))
            {
                throw new KeyNotFoundException($"No entry found for '{key}'.");
            }

            if (!GenericConverter.TryConvert(value, out T? result))
            {
                throw new ArgumentException($"Type mismatch. Value for '{key}' is not a valid '{typeof(T).Name}'.",
                    nameof(T));
            }

            return result!;
        }

        /// <summary>
        /// Gets a value of the specified type.
        /// </summary>
        /// <typeparam name="T">Expected type of the value.</typeparam>
        /// <typeparam name="TViewModel">Type of the view model this information is associated with.</typeparam>
        /// <param name="source">The repository to query.</param>
        /// <param name="key">The key associated with the value that should be retrieved.</param>
        /// <returns>An instance of <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="key"/> is <see langword="null"/> or empty.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        ///     If no entry was found for the specified <paramref name="key"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     If <paramref name="source"/> is <see langword="null"/>.
        ///     If the value is not of the expected type <typeparamref name="T"/>.
        /// </exception>
        public static T GetAs<TViewModel, T>(this IViewStateRepository source, string key)
        {
            ArgumentNullException.ThrowIfNull(source, nameof(source));
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

            if (!source.TryGet<TViewModel>(key, out string? value))
            {
                throw new KeyNotFoundException($"No entry found for '{key}'.");
            }

            if (!GenericConverter.TryConvert(value, out T? result))
            {
                throw new ArgumentException($"Type mismatch. Value for '{key}' is not a valid '{typeof(T).Name}'.",
                    nameof(T));
            }

            return result!;
        }

        /// <summary>
        /// Attempts to get a value of the specified type.
        /// </summary>
        /// <typeparam name="T">Expected type of the value.</typeparam>
        /// <param name="source">The repository to query.</param>
        /// <param name="key">The key associated with the value that should be retrieved.</param>
        /// <param name="value">
        ///     The successfully retrieved and converted value or <see langword="null"/> if either fails.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if a value could be successfully retrieved, otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryGetAs<T>(this IViewStateRepository source, string key, out T? value)
        {
            value = default;

            if (source == null)
            {
                return false;
            }

            if (String.IsNullOrEmpty(key))
            {
                return false;
            }

            if (!source.TryGet(key, out string? rawValue))
            {
                return false;
            }

            if (!GenericConverter.TryConvert(rawValue, out T? result))
            {
                return false;
            }

            value = result;
            return true;
        }

        /// <summary>
        /// Attempts to get a value of the specified type.
        /// </summary>
        /// <typeparam name="T">Expected type of the value.</typeparam>
        /// <typeparam name="TViewModel">Type of the view model this information is associated with.</typeparam>
        /// <param name="source">The repository to query.</param>
        /// <param name="key">The key associated with the value that should be retrieved.</param>
        /// <param name="value">
        ///     The successfully retrieved and converted value or <see langword="null"/> if either fails.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if a value could be successfully retrieved, otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryGetAs<TViewModel, T>(this IViewStateRepository source, string key, out T? value)
        {
            value = default;

            if (source == null)
            {
                return false;
            }

            if (String.IsNullOrEmpty(key))
            {
                return false;
            }

            if (!source.TryGet<TViewModel>(key, out string? rawValue))
            {
                return false;
            }

            if (!GenericConverter.TryConvert(rawValue, out T? result))
            {
                return false;
            }

            value = result;
            return true;
        }
    }
}
