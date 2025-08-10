using System;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Represents a score value issued by a juror.
    /// </summary>
    public sealed class ScoreEntry : Entity
    {
        /// <summary>
        /// Gets or sets the reference to the <see cref="ClubMember"/> identifying the juror who issued this score value.
        /// </summary>
        public Guid JurorId { get; set; }

        /// <summary>
        /// Gets or sets the score value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ScoreEntry"/>.
        /// </summary>
        public ScoreEntry()
        {
            JurorId = Guid.Empty;
            Value = 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, Juror='{JurorId}', Value='{Value}'";
        }
    }
}
