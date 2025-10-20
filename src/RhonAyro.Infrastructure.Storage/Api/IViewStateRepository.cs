namespace RhonAyro.Infrastructure.Storage.Api
{
    /// <summary>
    /// Manages and persists view state information.
    /// </summary>
    public interface IViewStateRepository : IFileRepository
    {
        /// <summary>
        /// Saves the <paramref name="value"/> with the specified <paramref name="key"/> for the specified view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model associated with this entry.</typeparam>
        /// <param name="key">Identifier for the entry.</param>
        /// <param name="value">The value to store.</param>
        void Save<TViewModel>(string key, string value);

        /// <summary>
        /// Saves the <paramref name="value"/> with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Identifier for the entry.</param>
        /// <param name="value">The value to store.</param>
        void Save(string key, string value);

        /// <summary>
        /// Attempts to retrieve a value from the store for the specified view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model associated with this entry.</typeparam>
        /// <param name="key">Identifier for the entry.</param>
        /// <param name="value">The value if retrieved successfully, otherwise <see langword="null"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if the value could be successfully retrieved, otherwise <see langword="false"/>.
        /// </returns>
        bool TryGet<TViewModel>(string key, out string? value);

        /// <summary>
        /// Attempts to retrieve a value from the store.
        /// </summary>
        /// <param name="key">Identifier for the entry.</param>
        /// <param name="value">The value if retrieved successfully, otherwise <see langword="null"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if the value could be successfully retrieved, otherwise <see langword="false"/>.
        /// </returns>
        bool TryGet(string key, out string? value);

        /// <summary>
        /// Removes the view state entry from the repository.
        /// </summary>
        /// <param name="key">The key of the entry to remove.</param>
        /// <returns><see langword="true"/> if the entry was removed, otherwise <see langword="false"/>.</returns>
        bool Remove(string key);

        /// <summary>
        /// Removes the view state entry from the repository for the specified view model.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model associated with entry to remove.</typeparam>
        /// <param name="key">The key of the entry to remove.</param>
        /// <returns><see langword="true"/> if the entry was removed, otherwise <see langword="false"/>.</returns>
        bool Remove<TViewModel>(string key);
    }
}
