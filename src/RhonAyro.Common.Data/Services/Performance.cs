using System;
using System.Collections.Generic;

namespace RhonAyro.Common.Data.Services
{
    public sealed class Performance
    {
        public Guid CompetitionId { get; }
        public float WheelSize { get; }
        public Guid DisciplineId { get; }
        public string Athlete { get; }
        public string Coach { get; }
        public int StartPosition { get; }
        public float Difficulty { get; }
        public float FinalScore { get; }
        public List<float> ScoreValues { get; }

        public Performance()
        {
            ScoreValues = new List<float>();
        }
    }
}
