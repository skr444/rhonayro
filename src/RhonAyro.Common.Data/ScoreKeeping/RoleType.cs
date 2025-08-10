
using System;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Enumerates different roles club members can have.
    /// </summary>
    [Flags]
    public enum RoleType
    {
        /// <summary>
        /// Member has currently no role associated.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Member is an athlete performing in the competition.
        /// </summary>
        Athlete = 1,

        /// <summary>
        /// Member assists athletes during their performances.
        /// </summary>
        Coach = 2,

        /// <summary>
        /// Member judges the performances of the athletes and assigns score values.
        /// </summary>
        Juror = 4,

        /// <summary>
        /// Member that oversees the compeition and is responsible for correctness of jury's procedures.
        /// </summary>
        HeadJuror = 8
    }
}
