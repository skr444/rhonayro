using System;
using System.Linq;

using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Runtime.Api;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Infrastructure.Storage.File
{
    /// <summary>
    /// Manages instances of <see cref="Logo"/>.
    /// </summary>
    internal sealed class LogoRepository : FileRepository<Logo>, ILogoRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="LogoRepository"/>.
        /// </summary>
        /// <param name="storageFilePath">Filesystem path pointing to the storage file.</param>
        /// <param name="lifecycleManager">Central lifecycle management.</param>
        public LogoRepository(string storageFilePath, ILifecycleManager lifecycleManager)
            : base(storageFilePath, lifecycleManager)
        {
        }

        /// <inheritdoc />
        public bool TryGet(string type, Guid competitionId, out Logo? logo)
        {
            logo = All().FirstOrDefault(x =>
                   (x.Type == type)
                && (x.CompetitionId == competitionId));

            return logo != null;
        }
    }
}
