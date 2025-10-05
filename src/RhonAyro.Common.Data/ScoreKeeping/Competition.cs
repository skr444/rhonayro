using System;
using System.Collections.Generic;

using RhonAyro.Common.Data.Extensions;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Represents a wheel gymnastics competition.
    /// </summary>
    public sealed class Competition : Entity
    {
        /// <summary>
        /// Gets or sets the date and time when this competition started.
        /// </summary>
        public DateTime EventStart { get; set; }

        /// <summary>
        /// Gets or sets a collection of <see cref="DisciplineSession"/> references.
        /// </summary>
        public ICollection<Guid> DisciplineSessions { get; set; }

        /// <summary>
        /// Gets or sets the reference to the head juror.
        /// </summary>
        public Guid HeadJuror { get; set; }

        /// <summary>
        /// Gets or sets the reference to the competition manager.
        /// </summary>
        public Guid CompetitionManager { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Competition"/>.
        /// </summary>
        public Competition()
        {
            EventStart = DateTime.UtcNow;
            DisciplineSessions = new HashSet<Guid>();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, EventStart='{EventStart}', DisciplineSessions='{DisciplineSessions.ToCustomString()}', HeadJuror='{HeadJuror}', CompetitionManager='{CompetitionManager}'";
        }
    }
}
