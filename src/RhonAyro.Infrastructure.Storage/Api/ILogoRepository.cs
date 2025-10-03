using System;

using RhonAyro.Common.Data.ScoreKeeping;

namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Provides means to manage instances of <see cref="Logo"/>.
    /// </summary>
    public interface ILogoRepository : IRepository<Logo>, IFileRepository
    {
    }
}
