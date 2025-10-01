namespace RhonAyro.Client.Desktop.Ui.Services
{
    /// <summary>
    /// Provides means to access internal resources.
    /// </summary>
    internal interface IResourceService
    {
        /// <summary>
        /// Gets the raw byte of a placeholder image.
        /// </summary>
        /// <returns>Binary data of the placeholder image.</returns>
        byte[] GetPlaceHolderImage();
    }
}
