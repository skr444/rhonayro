using System;
using System.Reflection;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    /// <inheritdoc/>
    internal sealed class ResourceService : IResourceService
    {
        private static readonly string ResourceNamespace = $"{nameof(RhonAyro.Client.Desktop.Ui)}.Resources";
        private static readonly string PlaceHolderImageResource = $"{ResourceNamespace}.placeholder-image.png";

        private static readonly Assembly assembly = Assembly.GetExecutingAssembly();

        /// <inheritdoc/>
        public byte[] GetPlaceHolderImage()
        {
            using (var stream = assembly?.GetManifestResourceStream(PlaceHolderImageResource))
            {
                if (stream is null)
                {
                    return Array.Empty<byte>();
                }

                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                return buffer;
            }
        }
    }
}
