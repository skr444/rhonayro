using System;
using System.Collections.Generic;

using RhonAyro.Common.Data.Extensions;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Holds data about a performance.
    /// </summary>
    public sealed class PerformanceEntry : Entity
    {
        /// <summary>
        /// Gets or sets the reference to the <see cref="StartListEntry"/> instance with the athlete data.
        /// </summary>
        public Guid StartListEntryId { get; set; }

        /// <summary>
        /// Gets or sets a list of references to <see cref="ScoreEntry"/> instances holding the achieved score values.
        /// </summary>
        public ICollection<Guid> ScoreEntries { get; set; }

        /// <summary>
        /// Gets or sets the difficulty level of this performance.
        /// </summary>
        public float Difficluty { get; set; }

        /// <summary>
        /// Gets or sets the value of the final score that is calculated when the performance has been completed.
        /// </summary>
        public float FinalScore { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="PerformanceEntry"/>.
        /// </summary>
        public PerformanceEntry()
        {
            ScoreEntries = new HashSet<Guid>();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, StartListEntryId='{StartListEntryId}', ScoreEntries='{ScoreEntries.ToCustomString()}', Difficluty='{Difficluty}', FinalScore='{FinalScore}'";
        }
    }
}
