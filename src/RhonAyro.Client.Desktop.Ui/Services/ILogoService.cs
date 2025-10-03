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
        Logo FirstOrDefault(Func<Logo, bool>? predicate = null);

        /// <summary>
        /// Attempts to retrieve an instance of <see cref="Logo"/>.
        /// </summary>
        /// <param name="type">Type label.</param>
        /// <param name="name">File name.</param>
        /// <param name="competitionId">Id of the associated competition.</param>
        /// <returns>The specified logo entry or a dummy entry with the placeholder image.</returns>
        Logo GetLogo(string type, string name, Guid competitionId);

        byte[] GetLogoData(Guid id);

        BitmapImage ToImage(byte[]? data);
    }
}
