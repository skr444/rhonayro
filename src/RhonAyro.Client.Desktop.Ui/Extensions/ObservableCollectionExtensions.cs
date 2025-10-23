using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RhonAyro.Client.Desktop.Ui.Extensions
{
    internal static class ObservableCollectionExtensions
    {
        public static void ReplaceWith<T>(this ObservableCollection<T> source, IEnumerable<T> newItems)
        {
            if (source == null)
            {
                return;
            }

            if (newItems == null)
            {
                return;
            }

            if (source.SequenceEqual(newItems))
            {
                return;
            }

            source.Clear();
            foreach (var item in newItems)
            {
                source.Add(item);
            }
        }
    }
}
