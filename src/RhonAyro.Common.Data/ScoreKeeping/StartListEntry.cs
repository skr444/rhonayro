using System;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Represents an entry in the athletes' line-up for the competition.
    /// </summary>
    public sealed class StartListEntry : Entity
    {
        /// <summary>
        /// Gets or sets the reference to the <see cref="Wheel"/> meta data instance.
        /// </summary>
        public Guid? WheelId { get; set; }

        /// <summary>
        /// Gets or sets the reference to the <see cref="ClubMember"/> instance of the athlete.
        /// </summary>
        public Guid? AthleteId { get; set; }

        /// <summary>
        /// Gets or sets the reference to the <see cref="ClubMember"/> instance of the coach.
        /// </summary>
        public Guid? CoachId { get; set; }

        /// <summary>
        /// Gets or sets the current start position in the line-up.
        /// </summary>
        public int StartPosition { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="StartListEntry"/>.
        /// </summary>
        public StartListEntry()
        {
            WheelId = null;
            AthleteId = null;
            CoachId = null;
            StartPosition = 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, WheelId='{WheelId}', AthleteId='{AthleteId}', CoachId='{CoachId}', StartPosition='{StartPosition}'";
        }
    }
}
