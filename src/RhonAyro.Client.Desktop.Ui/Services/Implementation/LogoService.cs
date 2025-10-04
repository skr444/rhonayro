using System;
using System.Linq;

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
        public Logo? FirstOrDefault(Func<Logo, bool>? predicate = null, bool useFallbackLogo = false)
        {
            Logo? logo = logoRepository.All().FirstOrDefault(predicate ?? (_ => true));

            if (logo == null)
            {
                return GetPlaceholderLogo();
            }

            return logo;
        }

        /// <inheritdoc />
        public Logo? GetLogo(string type, string name, Guid competitionId, bool useFallbackLogo = false)
        {
            return FirstOrDefault(x =>
                   (x.Type == type)
                && (x.Name == name)
                && (x.CompetitionId == competitionId),
                useFallbackLogo);
        }

        public Logo? GetLogo(string type, Guid competitionId, bool useFallbackLogo = false)
        {
            return FirstOrDefault(x =>
                   (x.Type == type)
                && (x.CompetitionId == competitionId),
                useFallbackLogo);
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

        public byte[] ToLogoBytes(byte[]? data)
        {
            if ((data?.Length ?? 0) == 0)
            {
                return resources.GetPlaceHolderImage();
            }

            return data!;
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
