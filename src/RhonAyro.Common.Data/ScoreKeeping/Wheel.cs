
namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Represents a wheel gymnastics wheel.
    /// </summary>
    public sealed class Wheel : Entity
    {
        /// <summary>
        /// Gets or sets the size of the wheel in [cm].
        /// </summary>
        public float Size { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, Size='{Size}'";
        }
    }
}
