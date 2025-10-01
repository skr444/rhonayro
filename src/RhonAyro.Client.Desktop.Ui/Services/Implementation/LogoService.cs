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
        public byte[] GetLogoData(string type, Guid competitionId)
        {
            Logo? logo = logoRepository.All().FirstOrDefault(x =>
                   (x.Type == type)
                && (x.CompetitionId == competitionId));

            if (logo == null)
            {
                return resources.GetPlaceHolderImage();
            }

            return logo.ImageData;
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
    }
}
