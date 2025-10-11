namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Represents a type of compeditive discipline of a performance.
    /// </summary>
    public sealed class Discipline : Entity
    {
        /// <summary>
        /// Gets or sets the name of the discipline.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Discipline"/>.
        /// </summary>
        public Discipline()
        {
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, Name='{Name}'";
        }
    }
}
