using System;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    public sealed class WheelAssignment : Entity
    {
        public Guid? WheelId { get; set; }

        public Guid? AthleteId { get; set; }

        public Guid? DisciplineId { get; set; }

        public WheelAssignment()
        {
            WheelId = null;
            AthleteId = null;
            DisciplineId = null;
        }
    }
}
