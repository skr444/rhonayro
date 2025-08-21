using System;
using System.Collections.Generic;
using System.IO;

using RhonAyro.Infrastructure.Runtime.Api;
using RhonAyro.Infrastructure.Storage.Api;

namespace RhonAyro.Infrastructure.Storage.File
{
    /// <summary>
    /// Manages file based repositories.
    /// </summary>
    internal sealed class FileStorage : IFileStorage
    {
        private readonly string storageDirectory;
        private readonly IDictionary<Type, IFileRepository> repositories;

        /// <inheritdoc/>
        public string StorageDirectory => storageDirectory;

        /// <summary>
        /// Creates a new instance of <see cref="FileStorage"/>.
        /// </summary>
        /// <param name="storageDir">Application data storage directory.</param>
        /// <param name="lifecycleManagement">Central lifecycle management.</param>
        public FileStorage(string storageDir, ILifecycleManager lifecycleManagement)
        {
            storageDirectory = storageDir;
            repositories = new Dictionary<Type, IFileRepository>();

            repositories.Add(typeof(IClubMemberRepository), new ClubMemberRepository(
                Path.Combine(storageDirectory, "clubMembers.json"),
                lifecycleManagement));

            repositories.Add(typeof(IDisciplineRepository), new DisciplineRepository(
                Path.Combine(storageDirectory, "disciplines.json"),
                lifecycleManagement));

            repositories.Add(typeof(IDisciplineSessionRepository), new DisciplineSessionRepository(
                Path.Combine(storageDirectory, "disciplineSessions.json"),
                lifecycleManagement));

            repositories.Add(typeof(IPerformanceEntryRepository), new PerformanceEntryRepository(
                Path.Combine(storageDirectory, "performances.json"),
                lifecycleManagement));

            repositories.Add(typeof(IScoreBoardRepository), new ScoreBoardRepository(
                Path.Combine(storageDirectory, "scoreBoards.json"),
                lifecycleManagement));

            repositories.Add(typeof(IScoreEntryRepository), new ScoreEntryRepository(
                Path.Combine(storageDirectory, "scoreEntries.json"),
                lifecycleManagement));

            repositories.Add(typeof(IStartListEntryRepository), new StartListEntryRepository(
                Path.Combine(storageDirectory, "startListEntries.json"),
                lifecycleManagement));

            repositories.Add(typeof(IWheelRepository), new WheelRepository(
                Path.Combine(storageDirectory, "wheels.json"),
                lifecycleManagement));
        }

        /// <inheritdoc/>
        public T GetRepository<T>()
        {
            var type = typeof(T);
            if (!repositories.TryGetValue(type, out IFileRepository? repo))
            {
                throw new ArgumentException($"No repository found for type '{type.Name}'.", nameof(T));
            }

            if (repo is not T candidate)
            {
                throw new InvalidOperationException($"Repository type mismatch! Expected type: '{type.Name}' but got '{repo.GetType().Name}'.");
            }

            return candidate;
        }

        /// <inheritdoc/>
        public void Load()
        {
            foreach (IFileRepository repo in repositories.Values)
            {
                repo.Load();
            }
        }

        /// <inheritdoc/>
        public void Save()
        {
            foreach (IFileRepository repo in repositories.Values)
            {
                repo.Save();
            }
        }

        /// <inheritdoc/>
        public void Delete()
        {
            foreach (IFileRepository repo in repositories.Values)
            {
                repo.Delete();
            }
        }
    }
}
