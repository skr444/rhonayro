using System;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="Logo"/>.
    /// </summary>
    public interface ILogoRepository : IRepository<Logo>, IFileRepository
    {
        /// <summary>
        /// Attempts to retrieve an instance of <see cref="Logo"/>.
        /// </summary>
        /// <param name="type">Type label.</param>
        /// <param name="competitionId">Id of the associated competition.</param>
        /// <param name="logo">The retrieved logo instance or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if an instance was found, otherwise <see langword="false"/>.</returns>
        bool TryGet(string type, Guid competitionId, out Logo? logo);
    }
}
