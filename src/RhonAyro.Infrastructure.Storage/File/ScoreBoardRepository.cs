using RhonAyro.Common.Data ScoreKeeping;
using RhonAyro.Infrastructure.Runtime Api;
using RhonAyro.Infrastructure.Storage Api;

namespace RhonAyro.Infrastructure.Storage File
{
    /// <summary>
    /// Manages instances of <see cref="ScoreBoard"/>.
    /// </summary>
    internal sealed class ScoreBoardRepository : FileRepository<ScoreBoard>, IScoreBoardRepository
    {
        /// <summary>
        /// Creates a new instance of <see cref="ScoreBoardRepository"/>.
        /// </summary>
        /// <param name="storageFilePath">Filesystem path pointing to the storage file.</param>
        /// <param name="lifecycleManagement">Central lifecycle management.</param>
        public ScoreBoardRepository(string storageFilePath, ILifecycleManager lifecycleManagement)
            : base(storageFilePath, lifecycleManagement)
        {
        }
    }
}
