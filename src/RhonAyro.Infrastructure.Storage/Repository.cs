using System;
using System.Collections.Generic;
using System.Linq;

using RhonAyro.Common.Data;

namespace RhonAyro.Infrastructure.Storage
{
    /// <summary>
    /// Stores instances of <typeparamref name="TData"/> in memory.
    /// </summary>
    /// <typeparam name="TData">Data type of the instance to persist.</typeparam>
    internal abstract class Repository<TData> : IRepository<TData> where TData : Entity
    {
        private readonly object locker;
        protected IDictionary<Guid, TData> store;

        public int Count => store.Count;

        protected Repository()
        {
            locker = new object();
            store = new Dictionary<Guid, TData>();
        }

        /// <inheritdoc />
        public void AddOrUpdate(TData data)
        {
            Validate(data);

            lock (locker)
            {
                store[data.Id] = data;
                data.Modified = DateTime.UtcNow;

                Persist();
            }
        }

        /// <inheritdoc />
        public ICollection<TData> All(Func<TData, bool>? predicate = null)
        {
            lock (locker)
            {
                Acquire();

                if (predicate == null)
                {
                    return store.Values;
                }

                return store.Values.Where(predicate).ToList();
            }
        }

        /// <inheritdoc />
        public void Delete(Guid id)
        {
            lock (locker)
            {
                if (store.Remove(id))
                {
                    Persist();
                }
            }
        }

        public int Delete(Func<TData, bool> predicate)
        {
            lock (locker)
            {
                int count = 0;
                foreach (var item in new List<TData>(store.Values.Where(predicate)))
                {
                    if (store.Remove(item.Id))
                    {
                        count++;
                    }
                }
                Persist();
                return count;
            }
        }

        /// <inheritdoc />
        public bool TryGet(Guid id, out TData? instance)
        {
            instance = null;

            lock (locker)
            {
                Acquire();

                return store.TryGetValue(id, out instance);
            }
        }

        /// <summary>
        /// Performs the necessary actions to persist the current store data.
        /// </summary>
        protected virtual void Persist()
        {
        }

        /// <summary>
        /// Performs the necessary actions to update the store with the latest persisted data version.
        /// </summary>
        protected virtual void Acquire()
        {
        }

        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="data">The instance to check.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/> has an invalid identifier.</exception>
        protected static void Validate(TData data)
        {
            ArgumentNullException.ThrowIfNull(data);
            if (data.Id == Guid.Empty)
            {
                throw new ArgumentException("Invalid id.", nameof(data));
            }
        }
    }
}
