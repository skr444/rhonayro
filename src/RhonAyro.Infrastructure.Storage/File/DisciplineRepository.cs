using RhonAyro.Common.Data.ScoreKeeping;
using RhonAyro.Infrastructure.Runtime.Api;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Infrastructure.Storage.File
{
    /// <summary>
    /// Manages instances of <see cref="Discipline"/>.
    /// </summary>
    internal sealed class DisciplineRepository : FileRepository<Discipline>, IDisciplineRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="DisciplineRepository"/>.
        /// </summary>
        /// <param name="storageFilePath">Filesystem path pointing to the storage file.</param>
        /// <param name="lifecycleManagement">Central lifecycle management.</param>
        public DisciplineRepository(string storageFilePath, ILifecycleManager lifecycleManagement)
            : base(storageFilePath, lifecycleManagement)
        {
        }
    }
}
