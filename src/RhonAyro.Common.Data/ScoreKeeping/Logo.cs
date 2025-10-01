using System;

namespace RhonAyro.Common.Data.ScoreKeeping
{
    /// <summary>
    /// Represents a logo image with path and raw data.
    /// </summary>
    public sealed class Logo : Entity
    {
        /// <summary>
        /// Gets or sets the raw image data.
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Gets or sets the type label of this logo.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the reference to the <see cref="Competition"/> instance.
        /// </summary>
        public Guid CompetitionId { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Logo"/>.
        /// </summary>
        public Logo()
        {
            ImageData = Array.Empty<byte>();
            Type = null;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{base.ToString()}, ImageDataBytes='{ImageData.Length}', Type='{Type}', CompetitionId='{CompetitionId}'";
        }
    }
}
