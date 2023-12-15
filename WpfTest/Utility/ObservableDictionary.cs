using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfTest.Utility
{
    public class ObservableDictionary<TKey, TValue> : ObservableCollection<KeyValuePair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool Remove(TKey key)
        {
            foreach (var item in Items)
            {
                if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
                {
                    return Remove(item);
                }
            }
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                foreach (var item in Items)
                {
                    if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
                    {
                        return item.Value;
                    }
                }
                throw new KeyNotFoundException();
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (EqualityComparer<TKey>.Default.Equals(Items[i].Key, key))
                    {
                        SetItem(i, new KeyValuePair<TKey, TValue>(key, value));
                        return;
                    }
                }
                Add(new KeyValuePair<TKey, TValue>(key, value));
            }
        }
        public bool ContainsKey(TKey key)
        {
            foreach (var item in Items)
            {
                if (EqualityComparer<TKey>.Default.Equals(item.Key, key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
