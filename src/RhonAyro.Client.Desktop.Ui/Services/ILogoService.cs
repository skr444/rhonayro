using System;

namespace RhonAyro.Client.Desktop.Ui.Services
{
    /// <summary>
    /// Provides means to manage logo images.
    /// </summary>
    internal interface ILogoService
    {
        byte[] GetLogoData(string type, Guid competitionId);

        byte[] GetLogoData(Guid id);
    }
}
