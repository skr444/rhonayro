using System;
using System.Collections.Generic;

using RhonAyro.Common.Data.Extensions;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Represents the execution of all performances of a specific discipline.
    /// </summary>
    public sealed class DisciplineSession : Entity
    {
        /// <summary>
        /// Gets or sets a reference to the <see cref="Discipline"/> instance.
        /// </summary>
        public Guid DisciplineId { get; set; }

        /// <summary>
        /// Gets or sets the reference to the <see cref="ClubMember"/> identifying the juror responsible for this session.
        /// </summary>
        public Guid? JurorId { get; set; }

        /// <summary>
        /// Gets or sets a collection of references to performances carried out during this session.
        /// </summary>
        public ICollection<Guid> Performances { get; set; }

        /// <summary>
        /// Gets or sets the reference to the <see cref="ScoreBoard"/> instance holding the current rankings of this session.
        /// </summary>
        public Guid ScoreBoardId { get; set; }

        /// <summary>
        /// Gets or sets a collection of start list entry references.
        /// </summary>
        public ICollection<Guid> StartList { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="DisciplineSession"/>.
        /// </summary>
        public DisciplineSession()
        {
            Performances = new HashSet<Guid>();
            StartList = new HashSet<Guid>();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, DisciplineId='{DisciplineId}', JurorId='{JurorId}', Performances='{Performances.ToCustomString()}', ScoreBoardId='{ScoreBoardId}', StartList='{StartList.ToCustomString()}'";
        }
    }
}
