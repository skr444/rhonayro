using System;
using System.Windows.Media.Imaging;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Client.Desktop.Ui.Services
{
    /// <summary>
    /// Provides means to manage logo images.
    /// </summary>
    internal interface ILogoService
    {
        Logo? FirstOrDefault(Func<Logo, bool>? predicate = null, bool useFallbackLogo = false);

        /// <summary>
        /// Attempts to retrieve an instance of <see cref="Logo"/>.
        /// </summary>
        /// <param name="type">Type label.</param>
        /// <param name="name">File name.</param>
        /// <param name="competitionId">Id of the associated competition.</param>
        /// <param name="useFallbackLogo">Whether to fall back to the placeholder image.</param>
        /// <returns>The specified logo entry or <see langword="null"/>.</returns>
        Logo? GetLogo(string type, string name, Guid competitionId, bool useFallbackLogo = false);

        Logo? GetLogo(string type, Guid competitionId, bool useFallbackLogo = false);

        byte[] GetLogoData(Guid id);

        byte[] ToLogoBytes(byte[]? data);
    }
}
