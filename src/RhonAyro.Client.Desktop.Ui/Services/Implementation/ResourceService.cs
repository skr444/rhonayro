using System;
using System.IO;
using System.Reflection;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    /// <inheritdoc/>
    internal sealed class ResourceService : IResourceService
    {
        private static readonly Assembly Assembly = typeof(ResourceService).Assembly;
        private static readonly string Root = Assembly.GetName().Name!; // "RhonAyro.Client.Desktop.Ui"
        private const string FileName = "placeholder-image.png";
        private static readonly string PlaceHolderImageResource = $"{Root}.Resources.{FileName}";

        /// <inheritdoc/>
        public byte[] GetPlaceHolderImage()
        {
            using var stream = Assembly.GetManifestResourceStream(PlaceHolderImageResource)
                ?? throw new InvalidOperationException(
                    $"Embedded resource '' not found. Available: {string.Join(", ", Assembly.GetManifestResourceNames())}");
            if (stream is null)
            {
                return Array.Empty<byte>();
            }

            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
