using System;
using System.Collections.Generic;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    public sealed class EventUnit : Entity
    {
        public ICollection<Guid> WheelAssignments { get; set; }

        public EventUnit()
        {
            WheelAssignments = [];
        }
    }
}
