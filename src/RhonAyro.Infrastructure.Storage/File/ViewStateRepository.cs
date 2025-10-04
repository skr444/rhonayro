using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using RhonAyro.Infrastructure.Runtime.Api;
using RhonAyro.Infrastructure.Storage.Api;

using IoFile = System.IO.File;

namespace RhonAyro.Infrastructure.Storage.File
{
    /// <inheritdoc />
    internal sealed class ViewStateRepository : IViewStateRepository
    {
        private readonly string path;
        private readonly JsonSerializerOptions serializeReadOptions;
        private readonly JsonSerializerOptions serializeWriteOptions;
        private Dictionary<string, string> store;
        private readonly ILifecycleManager lifecycleManager;

        /// <summary>
        /// Creates a new instance of <see cref="ViewStateRepository"/>.
        /// </summary>
        public ViewStateRepository(string storageFilePath, ILifecycleManager lifecycleManagement)
        {
            ArgumentException.ThrowIfNullOrEmpty(storageFilePath, nameof(storageFilePath));
            path = storageFilePath;

            ArgumentNullException.ThrowIfNull(lifecycleManagement, nameof(lifecycleManagement));
            lifecycleManager = lifecycleManagement;

            string? folder = Path.GetDirectoryName(path);
            if (!String.IsNullOrEmpty(folder) && !Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            serializeReadOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
            serializeWriteOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            store = new Dictionary<string, string>();
        }

        /// <inheritdoc />
        public void Save<TViewModel>(string key, string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

            store[$"{typeof(TViewModel).Name}.{key}"] = value;

            Save();
        }

        /// <inheritdoc />
        public void Save(string key, string value)
        {
            ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));

            store[key] = value;

            Save();
        }

        /// <inheritdoc />
        public bool TryGet<TViewModel>(string key, out string? value)
        {
            Load();

            return store.TryGetValue($"{typeof(TViewModel).Name}.{key}", out value);
        }

        /// <inheritdoc />
        public bool TryGet(string key, out string? value)
        {
            Load();

            return store.TryGetValue(key, out value);
        }

        /// <inheritdoc />
        public void Delete()
        {
            if (IoFile.Exists(path))
            {
                IoFile.Delete(path);
            }
        }

        /// <inheritdoc />
        public void Load()
        {
            if (IoFile.Exists(path))
            {
                Task.Run(async () =>
                {
                    using (Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read,
                        bufferSize: 8192, useAsync: true))
                    {
                        if (lifecycleManager.Token.IsCancellationRequested)
                        {
                            return;
                        }

                        store = await JsonSerializer
                            .DeserializeAsync<Dictionary<string, string>>(stream, serializeReadOptions, lifecycleManager.Token)
                                ?? new Dictionary<string, string>();
                    }
                }, lifecycleManager.Token).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            else
            {
                Save();
            }
        }

        /// <inheritdoc />
        public void Save()
        {
            Task.Run(async () =>
            {
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write,
                    bufferSize: 8192, useAsync: true))
                {
                    if (lifecycleManager.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await JsonSerializer.SerializeAsync(stream, store, serializeWriteOptions, lifecycleManager.Token);
                }
            }, lifecycleManager.Token).ConfigureAwait(true).GetAwaiter().GetResult();
        }
    }
}
