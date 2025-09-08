using System;

namespace RhonAyro.Common.Data.Ui
{
    /// <summary>
    /// Represents an information about a state of the UI.
    /// </summary>
    public sealed class ViewStateEntry : Entity
    {
        /// <summary>
        /// Gets or sets the identifier of this entry.
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// Gets or sets the value of this entry.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? View { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ViewStateEntry"/>.
        /// </summary>
        public ViewStateEntry()
        {
            Key = null;
            Value = null;
            View = null;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ViewStateEntry"/> from a string of the format
        /// <c>[key].[value]</c> or <c>[view].[key].[value]</c>
        /// </summary>
        /// <param name="instance">String representation of an instance of <see cref="ViewStateEntry"/>.</param>
        /// <exception cref="ArgumentException">If <paramref name="instance"/> is invalid.</exception>
        public ViewStateEntry(string instance)
        {
            string[] parts = instance.Split(".",
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            switch (parts.Length)
            {
                case 2:
                    Key = parts[0];
                    Value = parts[1];
                    View = null;
                    break;

                case 3:
                    Key = parts[1];
                    Value = parts[2];
                    View = parts[0];
                    break;

                default:
                    throw new ArgumentException($"Failed to parse view state entry '{instance}'", nameof(instance));
            }
        }

        /// <summary>
        /// Attempts to parse a string into an instance of <see cref="ViewStateEntry"/>.
        /// </summary>
        /// <param name="instance">The string to parse.</param>
        /// <param name="result">
        ///     The parsed <see cref="ViewStateEntry"/> instance or <see langword="null"/> if parsing failed.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="instance"/> was parsed successfully,
        ///     otherwise <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string instance, out ViewStateEntry? result)
        {
            result = null;

            try
            {
                result = new ViewStateEntry(instance);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return String.IsNullOrEmpty(View)
                ? $"{Key}.{Value}"
                : $"{View}.{Key}.{Value}";
        }
    }
}
