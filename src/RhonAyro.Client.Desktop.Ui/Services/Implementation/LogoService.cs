using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Client.Desktop.Ui.Services.Implementation
{
    /// <inheritdoc />
    internal sealed class LogoService : ILogoService
    {
        private readonly ILogoRepository logoRepository;
        private readonly IResourceService resources;

        public LogoService(IFileStorage fileStorage, IResourceService resourceService)
        {
            logoRepository = fileStorage.GetRepository<ILogoRepository>();
            resources = resourceService;
        }

        /// <inheritdoc />
        public Logo FirstOrDefault(Func<Logo, bool>? predicate = null)
        {
            return logoRepository.All().FirstOrDefault(predicate ?? (_ => true), GetPlaceholderLogo());
        }

        /// <inheritdoc />
        public Logo GetLogo(string type, string name, Guid competitionId)
        {
            return FirstOrDefault(x =>
                   (x.Type == type)
                && (x.Name == name)
                && (x.CompetitionId == competitionId));
        }

        /// <inheritdoc />
        public byte[] GetLogoData(Guid id)
        {
            if (logoRepository.TryGet(id, out Logo? logo))
            {
                return logo!.ImageData;
            }

            return resources.GetPlaceHolderImage();
        }

        /// <inheritdoc />
        public BitmapImage ToImage(byte[]? data)
        {
            byte[] candidateData;
            if ((data?.Length ?? 0) == 0)
            {
                candidateData = resources.GetPlaceHolderImage();
            }
            else
            {
                candidateData = data!;
            }

            using var stream = new MemoryStream(candidateData);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            // bmp.DecodePixelWidth = 512; // optional
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            return image;
        }

        private Logo GetPlaceholderLogo()
        {
            return new Logo
            {
                CompetitionId = Guid.Empty,
                Name = "tmp.png",
                Type = "Placeholder",
                ImageData = resources.GetPlaceHolderImage()
            };
        }
    }
}
