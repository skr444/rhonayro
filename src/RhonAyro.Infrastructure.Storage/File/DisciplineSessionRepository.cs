using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Runtime.Api;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Infrastructure.Storage.File
{
    /// <summary>
    /// Manages instances of <see cref="DisciplineSession"/>.
    /// </summary>
    internal sealed class DisciplineSessionRepository : FileRepository<DisciplineSession>, IDisciplineSessionRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="DisciplineSessionRepository"/>.
        /// </summary>
        /// <param name="storageFilePath">Filesystem path pointing to the storage file.</param>
        /// <param name="lifecycleManagement">Central lifecycle management.</param>
        public DisciplineSessionRepository(string storageFilePath, ILifecycleManager lifecycleManagement)
            : base(storageFilePath, lifecycleManagement)
        {
        }
    }
}
