using System;
using System.Collections.Generic;

using RhonAyro.Common.Data.Extensions;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Represents the score board for the ranking of a wheel gymnastics competition.
    /// </summary>
    public sealed class ScoreBoard : Entity
    {
        /// <summary>
        /// Gets or sets a collection of <see cref="PerformanceEntry"/> references.
        /// </summary>
        public ICollection<Guid> HighScore { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ScoreBoard"/>.
        /// </summary>
        public ScoreBoard()
        {
            HighScore = new HashSet<Guid>();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, HighScore='{HighScore.ToCustomString()}'";
        }
    }
}
